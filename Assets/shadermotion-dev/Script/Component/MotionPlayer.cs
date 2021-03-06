using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ShaderMotion {
public class MotionPlayer : MonoBehaviour  {
	[SerializeField]
	public RenderTexture motionBuffer;
	public Animator animator;
	public SkinnedMeshRenderer shapeRenderer;
	public int layer;
	public bool applyHumanPose; // for testing
	
	[System.NonSerialized]
	private GPUReader gpuReader = new GPUReader();
	[System.NonSerialized]
	private Skeleton skeleton;
	[System.NonSerialized]
	private MotionDecoder decoder;
	
	void OnEnable() {
		if(!animator)
			animator = GetComponent<Animator>();
		if(!shapeRenderer)
			shapeRenderer = (animator?.GetComponentsInChildren<SkinnedMeshRenderer>() ?? new SkinnedMeshRenderer[0])
				.Where(smr => (smr.sharedMesh?.blendShapeCount??0) > 0).FirstOrDefault();

		skeleton = new Skeleton(animator);
		var appr = new Appearance(shapeRenderer?.sharedMesh, true);
		var layout = new MotionLayout(skeleton, MotionLayout.defaultHumanLayout,
										appr, MotionLayout.defaultExprLayout);
		decoder = new MotionDecoder(skeleton, appr, layout);
			//motionBuffer = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
		}
		void OnDisable() {
		skeleton = null;
		decoder = null;
	}
	void Update() {
        //Debug.Log("texture buffer");
        //Debug.Log(motionBuffer);
		var request = gpuReader.Request(motionBuffer);
		if(request != null && !request.Value.hasError) {
            //Debug.Log("OHOHOH");
			decoder.Update(request.Value, layer);
			if(applyHumanPose)
				ApplyHumanPose();
			else
				ApplyTransform();
			ApplyBlendShape();
		} else if (request != null)
        {
            //Debug.Log(request.Value);
            //Debug.Log(request.Value.height);
            //Debug.Log(request.Value.width);
            //Debug.Log(request.Value.done);
        }
	}

	const float shapeWeightEps = 0.1f;
	const float rootScaleEps = 0.01f;
	private HumanPoseHandler poseHandler;
	private HumanPose humanPose;
	private Vector3[] swingTwists;
	void ApplyScale() {
		// var localScale = Mathf.Round(decoder.motions[0].s / skeleton.humanScale / rootScaleEps) * rootScaleEps;
		var localScale = decoder.motions[0].s / skeleton.humanScale;
		skeleton.root.localScale = Vector3.one * localScale;
	}
	void ApplyHumanPose() {
		if(poseHandler == null) {
			poseHandler = new HumanPoseHandler(skeleton.root.GetComponent<Animator>().avatar, skeleton.root);
			poseHandler.GetHumanPose(ref humanPose);
		}
		var motions = decoder.motions;
		System.Array.Resize(ref swingTwists, HumanTrait.BoneCount);
		for(int i=0; i<HumanTrait.BoneCount; i++)
			swingTwists[i] = motions[i].t;
		HumanPoser.SetBoneSwingTwists(ref humanPose, swingTwists);
		HumanPoser.SetHipsPositionRotation(ref humanPose, motions[0].t, motions[0].q, motions[0].s);
		poseHandler.SetHumanPose(ref humanPose);
		ApplyScale();
	}
	void ApplyTransform() {
		ApplyScale();
		skeleton.bones[0].position = skeleton.root.TransformPoint(decoder.motions[0].t / skeleton.root.localScale.y);
		for(int i=0; i<skeleton.bones.Length; i++)
			if(skeleton.bones[i]) {
				var axes = skeleton.axes[i];
				if(!skeleton.dummy[i])
					skeleton.bones[i].localRotation = axes.preQ * decoder.motions[i].q * Quaternion.Inverse(axes.postQ);
				else // TODO: this assumes non-dummy precedes dummy bone, so it fails on Neck
					skeleton.bones[i].localRotation *= axes.postQ * decoder.motions[i].q * Quaternion.Inverse(axes.postQ);
			}
	}
	void ApplyBlendShape() {
		if(shapeRenderer) {
			var mesh = shapeRenderer.sharedMesh;
			foreach(var kv in decoder.shapes) {
				var shape = mesh.GetBlendShapeIndex(kv.Key);
				if(shape >= 0)
					shapeRenderer.SetBlendShapeWeight(shape,
						Mathf.Round(Mathf.Clamp01(kv.Value)*100/shapeWeightEps)*shapeWeightEps);
			}
		}
	}
}
}