using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace nullbloq.Noodles
{
	/// <summary>
	/// Unity Window Object.
	/// Manages Noodle Graph view based on current selected Noodle
	/// Also Creates new noodles.
	/// </summary>
	public class NoodlesGraphWindow : EditorWindow
	{
		enum NoodlesWindowState
		{
			NoNoodleSelected,
			MultipleNoodlesSelected,
			NoodlerWithoutNoodle,
			NoodleSelected
		}

		private NoodlesWindowState windowState = NoodlesWindowState.NoNoodleSelected;

		private NoodlesGraphView _graphView;
		private Noodle currentNoodle;

		private static bool isUnityQuitting = false;
		/// <summary>
		/// Creates this Unity Window
		/// </summary>
		[MenuItem("Noodles/Noodles Window")]
		public static void CreateGraphViewWindow()
		{
			var w = GetWindow<NoodlesGraphWindow>();
			w.titleContent = new GUIContent("Noodles");
		}

		void Debug_DoSomething()
		{
			Debug.Log("DO something");
			if (_graphView == null)
			{
				Debug.Log("No GraphView");
				return;
			}

			foreach (Edge graphViewEdge in _graphView.edges.ToList())
			{
				Debug.Log(graphViewEdge.name);
				//Debug.Log(graphViewEdge.input.
			}

		}

		void OnDestroy()
		{
			SaveNoodle();
		}

		void OnLostFocus()
		{
			SaveNoodle();
		}

		/// <summary>
		/// Happens when object selection changed in Project Tab
		/// </summary>
		void OnSelectionChange()
		{
			RefreshNoodlesWindow();
		}
		private void OnEnable()
		{
			RefreshNoodlesWindow();
		}

		void SetNoodlesWindowState(NoodlesWindowState nws)
		{
			windowState = nws;
			RebuildUI();
		}

		/// <summary>
		/// Decides what to do in the window based on the selected Asset (Project View) or GameObject that contains a Noodler Component (Hierarchy)
		/// </summary>
		void RefreshNoodlesWindow()
		{
			Noodle selectedNoodle = null;

			//Check Selected Noodle SO in Project View
			Noodle[] selectedNoodles = Selection.GetFiltered<Noodle>(SelectionMode.Assets);
			if (selectedNoodles != null && selectedNoodles.Length > 0)
			{
				if (selectedNoodles.Length == 1)
					selectedNoodle = selectedNoodles[0];
				else
				{
					currentNoodle = null;
					SetNoodlesWindowState(NoodlesWindowState.MultipleNoodlesSelected);
					return;
				}
			}

			//Check Selected Noodler in Hierarchy
			Noodler[] noodlersSelected = Selection.GetFiltered<Noodler>(SelectionMode.Unfiltered);
			if (noodlersSelected != null && noodlersSelected.Length > 0)
			{
				if (noodlersSelected.Length == 1)
				{
					if (noodlersSelected[0].controller)
						selectedNoodle = noodlersSelected[0].controller;
					else
					{
						currentNoodle = null;
						SetNoodlesWindowState(NoodlesWindowState.NoodlerWithoutNoodle);
						return;
					}
				}
				else
				{
					currentNoodle = null;
					SetNoodlesWindowState(NoodlesWindowState.MultipleNoodlesSelected);
					return;
				}
			}

			//Check if valid Noodle & show it
			if (selectedNoodle && selectedNoodle != currentNoodle)
				currentNoodle = selectedNoodle;

			if (currentNoodle)
				SetNoodlesWindowState(NoodlesWindowState.NoodleSelected);
			else
				SetNoodlesWindowState(NoodlesWindowState.NoNoodleSelected);

			//Debug.Log(currentNoodle);
			//Debug.Log(currentNoodle.nodes.Count);
			//foreach (NoodlesNode node in currentNoodle.nodes)
			//{
			//	Debug.Log(node.title);
			//	//Debug.Log(node.);
			//}
		}

		void RebuildUI()
		{
			//Debug.Log("Redraw");
			rootVisualElement.Clear();
			rootVisualElement.styleSheets.Add(Resources.Load<StyleSheet>("NoodlesGraph"));
			switch (windowState)
			{
				case NoodlesWindowState.NoNoodleSelected:
					NoNoodleSelectedView();
					break;
				case NoodlesWindowState.MultipleNoodlesSelected:
					NoMultipleNoodleSelectionView();
					break;
				case NoodlesWindowState.NoodlerWithoutNoodle:
					NoodlerWithoutNoodleView();
					break;
				case NoodlesWindowState.NoodleSelected:
					ConstructGraphView();
					GenerateToolbar();
					GenerateMiniMap();
					GenerateBlackBoard();
					break;
			}
		}

		private void NoodlerWithoutNoodleView()
		{
			var errMsg = new Label("Noodler without Noodle, assign a Noodle to the Noodler Component");
			rootVisualElement.Add(errMsg);
		}

		private void NoMultipleNoodleSelectionView()
		{
			var errMsg = new Label("No multiple noodle edition available");
			rootVisualElement.Add(errMsg);
		}

		private void NoNoodleSelectedView()
		{
			var errMsg = new Label("No Noodle Selected, should you create one?");
			rootVisualElement.Add(errMsg);
			rootVisualElement.Add(new Button(CreateNewNoodle) { text = "Create Noodle" });
		}

		private void ConstructGraphView()
		{
			_graphView = new NoodlesGraphView(this) { name = "Noodles Graph View (" + currentNoodle.name + ")"};
			_graphView.StretchToParentSize();
			_graphView.LoadFromNoodle(currentNoodle);
			_graphView.OnGotDirty += RebuildUI;
			rootVisualElement.Add(_graphView);
		}

		private void GenerateToolbar()
		{
			var toolbar = new Toolbar();

			//var fileNameTextField = new Label("Noodle: " + currentNoodle.name);
			//fileNameTextField.styleSheets.Add(Resources.Load<StyleSheet>("NoodlesGraph"));
			//toolbar.Add(fileNameTextField);

			//toolbar.Add(new Button(SaveNoodle) { text = "Save" });
			toolbar.Add(new Button(Debug_DoSomething) { text = "Do Something" });
			
			//toolbar.Add(new Button(() => RequestDataOperation(false)) { text = "Load Data" });
			//toolbar.Add(new Button(() => _graphView.CreateNewStatIDNode("Stat")) { text = "New Stat Node", });
			rootVisualElement.Add(toolbar);
		}

		private void GenerateMiniMap()
		{
			var miniMap = new MiniMap { anchored = true };
			var cords = _graphView.contentViewContainer.WorldToLocal(new Vector2(this.maxSize.x, 45));
			miniMap.SetPosition(new Rect(cords.x, cords.y, 200, 140));
			_graphView.Add(miniMap);
		}

		private void GenerateBlackBoard()
		{
			var blackboard = new Blackboard(_graphView);
			blackboard.title = currentNoodle.name;
			blackboard.subTitle = "Noodle";
			blackboard.Add(new BlackboardSection { title = "Properties" });
			blackboard.styleSheets.Add(Resources.Load<StyleSheet>("Blackboard"));
			//blackboard.addItemRequested = _blackboard =>
			//{
			//	_graphView.AddPropertyToBlackBoard(ExposedProperty.CreateInstance(), false);
			//};
			//blackboard.editTextRequested = (_blackboard, element, newValue) =>
			//{
			//	var oldPropertyName = ((BlackboardField)element).text;
			//	if (_graphView.ExposedProperties.Any(x => x.PropertyName == newValue))
			//	{
			//		EditorUtility.DisplayDialog("Error", "This property name already exists, please chose another one.",
			//			"OK");
			//		return;
			//	}
			//
			//	var targetIndex = _graphView.ExposedProperties.FindIndex(x => x.PropertyName == oldPropertyName);
			//	_graphView.ExposedProperties[targetIndex].PropertyName = newValue;
			//	((BlackboardField)element).text = newValue;
			//};
			blackboard.SetPosition(new Rect(10, 30, 200, 300));
			_graphView.Add(blackboard);
			_graphView.Blackboard = blackboard;
		}

		//private void OnDisable()
		//{
		//	rootVisualElement.Remove(_graphView);
		//}

		void SaveNoodle()
		{
			if (!currentNoodle || isUnityQuitting)
			{
				//Debug.LogError("No noodle to save");
				return;
			}
			
			_graphView.Save_MOMENTARY_();

			AssetDatabase.SaveAssets(); //TODO check if needed.
			EditorUtility.SetDirty(currentNoodle);
			AssetDatabase.SaveAssets();
		}

		#region Helpers
		/// <summary>
		/// Process of creating a new Noodle SO
		/// </summary>
		[MenuItem("Assets/Create/Noodle")]
		private static void CreateNewNoodle()
		{
			string path = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (path == "")
			{
				path = "Assets";
			}
			else if (Path.GetExtension(path) != "")
			{
				path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
			}

			Noodle newNoodle = CreateInstance<Noodle>();
			newNoodle.name = "Noodle " + DateTime.Now.ToString("dd MMM yyyy hh:mm:ss").Replace(":", "_");
			newNoodle.name = newNoodle.name.Replace("-", "");

			NoodlesNodeBorder startNode = new NoodlesNodeBorder
			{
				isStartNode = true,
				title = "Start",
			};
			startNode.Init(new Vector2(300,200), Guid.NewGuid().ToString());

			NoodlesNodeBorder endNode = new NoodlesNodeBorder
			{
				isStartNode = false,
				title = "End",
			};
			endNode.Init(new Vector2(500, 200), Guid.NewGuid().ToString());

			newNoodle.nodes.Add(startNode);
			newNoodle.nodes.Add(endNode);

			AssetDatabase.CreateAsset(newNoodle, path + "/" + newNoodle.name + ".asset");

			AssetDatabase.SaveAssets();
			EditorUtility.SetDirty(newNoodle);
			CreateGraphViewWindow();
		}
		#endregion Helpers

	}
}