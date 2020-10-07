using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine.UIElements;

namespace nullbloq.Noodles
{
	[Serializable]
	public class NoodlesPort
	{
		public string GUID;
		public string parentNodeGUID;
		public List<string> targetNodeGUID = new List<string>();
		public List<string> targetPortGUID = new List<string>();
		public string text;
		public bool isEditable;

		public NoodlesPort(string _GUID, string _parentNodeGUID, string _text, bool _isEditable = false)
		{
			GUID = _GUID;
			parentNodeGUID = _parentNodeGUID;
			text = _text;
			isEditable = _isEditable;
		}

		public void ConnectToPort(NoodlesPort connectTo)
		{
			targetNodeGUID.Add(connectTo.parentNodeGUID);
			targetPortGUID.Add(connectTo.GUID);
		}

		public void RemoveConnectionToPort(string _portGUID)
		{
			int index = targetPortGUID.IndexOf(_portGUID);
			if (index<0)
				return;
			targetPortGUID.RemoveAt(index);
			targetNodeGUID.RemoveAt(index);
		}
	}
#if UNITY_EDITOR
	public class NoodlesPortVisual : Port
	{
		public NoodlesPort portData;
		public NoodlesPortVisual(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type)
			: base(portOrientation, portDirection, portCapacity, type)
		{
		}

		public void InitializeWithNoodlesNode(NoodlesPort _portData, Action<NoodlesPort, NoodlesPortVisual> removeCallback)
		{
			portData = _portData;
			portName = portData.text;
			if (portData.isEditable)
			{
				var textField = new TextField()
				{
					name = string.Empty,
					value = portData.text
				};
				textField.RegisterValueChangedCallback(evt => portData.text = evt.newValue);
				contentContainer.Add(textField);
				var deleteButton = new Button(() => removeCallback(portData, this))
				{
					text = "X"
				};
				contentContainer.Add(deleteButton);
				portName = "";
			}
		}

		public static NoodlesPortVisual CreateVisualPort(NoodlesPort _portData, Orientation portOrientation, Direction nodeDirection, Port.Capacity capacity, Type type, IEdgeConnectorListener connectorListener)
		{
			var newPort = new NoodlesPortVisual(portOrientation, nodeDirection, capacity, type)
			{
				m_EdgeConnector = new EdgeConnector<Edge>(connectorListener),
				portData = _portData,
			};
			newPort.AddManipulator(newPort.m_EdgeConnector);
			return newPort;
		}
	}
#endif
}
