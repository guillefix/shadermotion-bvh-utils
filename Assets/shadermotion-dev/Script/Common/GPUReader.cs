using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using AsyncGPUReadback = UnityEngine.Rendering.AsyncGPUReadback;
using AsyncGPUReadbackRequest = UnityEngine.Rendering.AsyncGPUReadbackRequest;

namespace ShaderMotion {
public class GPUReader {
	Queue<AsyncGPUReadbackRequest> requests = new Queue<AsyncGPUReadbackRequest>();
	public AsyncGPUReadbackRequest? Request(Texture tex) {
		AsyncGPUReadbackRequest? request = null;
        //Debug.Log(requests.Count);
        while (requests.Count > 0)
        {
            var r = requests.Peek();
            if (!r.done)
                break;
            request = requests.Dequeue();
        }
        //while (requests.Count > 1)
        //{
        //    var r = requests.Dequeue();
        //    if (r.done)
        //    {
        //        request = r;
        //    }
        //}
        //if (requests.Count > 0)
        //    if (requests.Peek().done)
        //        request = requests.Dequeue();

        //Debug.Log(requests.Count);
		if(requests.Count < 2)
            {
                requests.Enqueue(AsyncGPUReadback.Request(tex));
                //Debug.Log(requests.Peek().done);
            }
		return request;
	}
}
}