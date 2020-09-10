using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nullbloq.Noodles
{
	/// <summary>
	/// Noodle Model
	/// </summary>
	public class Noodle : ScriptableObject
	{
		[SerializeReference]
		public List<NoodlesNode> nodes = new List<NoodlesNode>();

		public NoodlesNode GetStartNode()
		{
			foreach (NoodlesNode node in nodes)
			{
				NoodlesNodeBorder border = node as NoodlesNodeBorder;
				if (border != null && border.isStartNode)
					return GetNode(node.GetNextNodeID(0)); //TODO change when node cache is coded
			}
			return null;
		}

		public NoodlesNode GetNode(string _guid)
		{
			for (var i = 0; i < nodes.Count; i++)
			{
				NoodlesNode node = nodes[i];
				if (node.GUID == _guid)
					return node;
			}

			return null;
		}

		public void RemoveNode(NoodlesNode n)
		{
			nodes.Remove(n);
		}
	}
}
