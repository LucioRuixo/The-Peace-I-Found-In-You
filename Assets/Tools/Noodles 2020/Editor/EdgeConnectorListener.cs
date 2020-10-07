using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace nullbloq.Noodles
{
	//TODO XXX Remove class YES REMOVE ME PLEASE
	public class EdgeConnectorListener : IEdgeConnectorListener
	{
		public void OnDropOutsidePort(Edge edge, Vector2 position)
		{
			//Debug.Log("EdgeConnectorListener OnDropOutsidePort: " + position);
		}

		public void OnDrop(GraphView graphView, Edge edge)
		{
			//Debug.Log("graphView: " + graphView.name);
			//Debug.Log("EdgeConnectorListener OnDropOutsidePort: " + graphView.name);
		}
	}
}
