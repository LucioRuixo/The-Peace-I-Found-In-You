using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace nullbloq.Noodles
{
	public class NoodlesGraphView : GraphView, IEdgeConnectorListener
	{
		public Noodle noodle;

		public List<NoodlesNodeVisual> visualAddedNodes;
		public Blackboard Blackboard = new Blackboard();
		private NodeSearchWindow searchWindow;
		private NoodlesGraphWindow editorWindow;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_editorWindow"></param>
		public NoodlesGraphView(NoodlesGraphWindow _editorWindow)
		{
			editorWindow = _editorWindow;
			visualAddedNodes = new List<NoodlesNodeVisual>();
			styleSheets.Add(Resources.Load<StyleSheet>("NoodlesGraph"));
			SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());
			this.AddManipulator(new FreehandSelector());

			graphViewChanged = OnGraphViewChanged;

			var grid = new GridBackground();
			Insert(0, grid);
			grid.StretchToParentSize();

			AddSearchWindow(editorWindow);
		}

		public void LoadFromNoodle(Noodle _noodle)
		{
			noodle = _noodle;
			LoadNodeViews();
		}

		void LoadNodeViews()
		{
			if (!noodle)
			{
				Debug.LogError("noodle doesn't exist");
				return;
			}

			foreach (NoodlesNode n in noodle.nodes)
			{
				//Debug.Log("node: " + n);
				CreateVisualNode(n);
			}

			//TODO add all edges
			foreach (NoodlesNodeVisual nv in visualAddedNodes)
			{
				//Debug.Log("node: " + n);
				//CreateVisualNode(n);
				foreach (NoodlesPortVisual npv in nv.inputPorts)
				{
					CreateEdgesForPort(npv);
				}
			}
		}

		private void AddSearchWindow(NoodlesGraphWindow editorWindow)
		{
			searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
			searchWindow.Configure(editorWindow, this);
			nodeCreationRequest = context =>
				SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
		}

		// NODES 

		public void AddNodeToNoodle(NoodlesNode n)
		{
			noodle.nodes.Add(n);
			CreateVisualNode(n);
		}

		NoodlesNodeVisual CreateVisualNode(NoodlesNode n)
		{
			NoodlesNodeVisual nnv = (NoodlesNodeVisual)Activator.CreateInstance(Type.GetType(n.classNameString));
			nnv.listener = this;
			nnv.InitializeWithNoodlesNode(n);
			//Debug.Log("Adding element");
			AddElement(nnv);
			visualAddedNodes.Add(nnv);
			return nnv;
		}

		void CreateEdgesForPort(NoodlesPortVisual sourcePortVisual)
		{
			NoodlesPort sourcePort = sourcePortVisual.portData;
			for (var i = 0; i < sourcePort.targetNodeGUID.Count; i++)
			{
				string targetNodeGuid = sourcePort.targetNodeGUID[i];
				NoodlesNode targetNode = noodle.GetNode(targetNodeGuid);
				NoodlesNodeVisual targetNodeVisual = GetNodeVisual(targetNodeGuid);
				string targetPortGuid = sourcePort.targetPortGUID[i];
				NoodlesPortVisual targetPortVisual = targetNodeVisual.GetPortWithGUID(targetPortGuid);
				Edge e = new Edge
				{
					input = sourcePortVisual,
					output = targetPortVisual
				};
				AddEdge(e);
			}
		}

		NoodlesNodeVisual GetNodeVisual(string nodeGUID)
		{
			for (var i = 0; i < visualAddedNodes.Count; i++)
			{
				NoodlesNodeVisual nodeVisual = visualAddedNodes[i];
				if (nodeVisual.nodeData.GUID == nodeGUID)
					return nodeVisual;
			}

			return null;
		}

		// Ports connections

		bool SetNodeConnection(Edge edge)
		{
			NoodlesNodeVisual outputNode = edge.output.node as NoodlesNodeVisual;
			NoodlesNodeVisual inputNode = edge.input.node as NoodlesNodeVisual;

			if (inputNode == null || outputNode == null)
			{
				Debug.LogError("Something went wrong");
				return false;
			}

			//Debug.Log("outputNode: " + outputNode.title);
			//Debug.Log("inputNode: " + inputNode.title);

			NoodlesPortVisual outputPort = edge.output as NoodlesPortVisual;
			NoodlesPortVisual inputPort = edge.input as NoodlesPortVisual;

			if (outputPort == null || inputPort == null)
			{
				Debug.LogError("Something went wrong");
				return false;
			}

			//Debug.Log("outputPort: " + outputPort.portData.text);
			//Debug.Log("inputPort: " + inputPort.portData.text);

			outputPort.portData.ConnectToPort(inputPort.portData);
			inputPort.portData.ConnectToPort(outputPort.portData);

			return true;
			//noodle.GetNode(edge.input.node)
		}

		void AddEdge(Edge edge)
		{
			if (edge == null || edge.input == null || edge.output == null)
			{
				Debug.LogError("Error adding edge: " + edge);
				return;
			}

			AddElement(edge);
			edge.input.ConnectTo(edge.output);
		}

		public void Save_MOMENTARY_()
		{
			foreach (NoodlesNodeVisual node in visualAddedNodes)
			{
				node.Save_MOMENTARY_();
			}
		}

		GraphViewChange OnGraphViewChanged(GraphViewChange change)
		{
			bool refreshView = false;
			if (change.elementsToRemove != null)
			{
				foreach (var element in change.elementsToRemove)
				{
					if (element is NoodlesNodeVisual node)
					{
						RemoveNode(node);
						refreshView = true;
					}
					else if (element is Edge edge)
					{
						RemoveEdge(edge);
						refreshView = true;
					}
				}
			}

			if (refreshView)
			{

			}

			return change;
		}

		private void RemoveEdge(Edge edge)
		{
			
			NoodlesPortVisual sourcePortVisual = edge.input as NoodlesPortVisual;
			NoodlesPortVisual targePortVisual = edge.output as NoodlesPortVisual;

			if (sourcePortVisual == null || targePortVisual == null)
			{
				Debug.Log("ARGGGGGQQQ!!!!!!");
				return;
			}

			NoodlesPort sourcePort = sourcePortVisual.portData;
			//NoodlesNode sourceNode = noodle.GetNode(sourcePort.parentNodeGUID);
			//NoodlesNodeVisual sourceNodeVisual = GetNodeVisual(sourceNode.GUID);
			NoodlesPort targePort = targePortVisual.portData;
			//NoodlesNode targetNode = noodle.GetNode(targePort.parentNodeGUID);
			//NoodlesNodeVisual targetNodeVisual = GetNodeVisual(targetNode.GUID);

			sourcePort.RemoveConnectionToPort(targePort.GUID);
			targePort.RemoveConnectionToPort(sourcePort.GUID);

			//Debug.Log("remove EDGE");
			//edge.input.Disconnect(edge);
			//edge.output.Disconnect(edge);
			//edge.input = null;
			//edge.output = null;
			RemoveElement(edge);
			//throw new NotImplementedException();
		}

		//TODO clean up
		//TODO refresh whole window
		private void RemoveNode(NoodlesNodeVisual node)
		{
			Debug.Log("remove NODE");
			foreach (NoodlesPortVisual inputPortVisual in node.inputPorts)
			{
				NoodlesPort inputPort = inputPortVisual.portData;
				for (var i = 0; i < inputPort.targetPortGUID.Count; i++)
				{
					string targetPortGUID = inputPort.targetPortGUID[i];
					string targetNodeGUID = inputPort.targetNodeGUID[i];

					NoodlesNode targetNode = noodle.GetNode(targetNodeGUID);
					NoodlesPort targetPort = targetNode.GetPort(targetPortGUID);
					targetPort.RemoveConnectionToPort(inputPort.GUID);
				}
			}

			foreach (NoodlesPortVisual inputPortVisual in node.outputPorts)
			{
				NoodlesPort inputPort = inputPortVisual.portData;
				for (var i = 0; i < inputPort.targetPortGUID.Count; i++)
				{
					string targetPortGUID = inputPort.targetPortGUID[i];
					string targetNodeGUID = inputPort.targetNodeGUID[i];

					NoodlesNode targetNode = noodle.GetNode(targetNodeGUID);
					NoodlesPort targetPort = targetNode.GetPort(targetPortGUID);
					targetPort.RemoveConnectionToPort(inputPort.GUID);
				}
			}
			noodle.RemoveNode(node.nodeData);
		}

		// ================================================ PORTS ============================================================

		public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
		{
			var compatiblePorts = new List<Port>();
			var startPortView = startPort;

			ports.ForEach((port) =>
			{
				var portView = port;
				if (startPortView != portView && startPortView.node != portView.node)
					compatiblePorts.Add(port);
			});

			return compatiblePorts;
		}

		public void OnDropOutsidePort(Edge edge, Vector2 position)
		{
			Debug.Log("EdgeConnectorListener OnDropOutsidePort: " + position);
		}

		public void OnDrop(GraphView graphView, Edge edge)
		{
			bool connectionMade = SetNodeConnection(edge);
			if (connectionMade)
				AddEdge(edge);
		}
	}
}