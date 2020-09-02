using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace nullbloq.Noodles
{
    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private EditorWindow _window;
        private NoodlesGraphView _graphView;

        private Texture2D _indentationIcon;
        
        public void Configure(EditorWindow window, NoodlesGraphView graphView)
        {
            _window = window;
            _graphView = graphView;
            
            //Transparent 1px indentation icon as a hack
            _indentationIcon = new Texture2D(1,1);
            _indentationIcon.SetPixel(0,0,new Color(0,0,0,0));
            _indentationIcon.Apply();
        }


		private Dictionary<string, Type> possibleNodes;
	    void PopulateNodeMenu()
	    {
		    possibleNodes = new Dictionary<string, Type>();
		    Type derivedType = typeof(NoodlesNode);
		    Assembly assembly = Assembly.GetAssembly(derivedType);

		    List<Type> nodeTypes = assembly
			    .GetTypes()
			    .Where(t =>
				    t != derivedType &&
				    derivedType.IsAssignableFrom(t)
			    ).ToList();

			//Debug.Log("nodeTypes: " + nodeTypes.Count);

		    foreach (Type nodeType in nodeTypes)
		    {
			    string niceNodeName = ObjectNames.NicifyVariableName(nodeType.Name);
			    //Debug.Log("niceNodeName: " + niceNodeName);
				possibleNodes.Add(niceNodeName, nodeType);
		    }
	    }

		public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
		{
			PopulateNodeMenu();

			var tree = new List<SearchTreeEntry>();

			tree.Add(new SearchTreeGroupEntry(new GUIContent("Create Node"), 0));
			tree.Add(new SearchTreeGroupEntry(new GUIContent("Nodes"), 1));

			//Add nodes
			foreach (KeyValuePair<string, Type> pair in possibleNodes)
	        {
		        SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(pair.Key, _indentationIcon))
				{
					level = 2,
					userData = Activator.CreateInstance(pair.Value)
				};
				tree.Add(entry);
			}

			//Add others
			tree.Add(new SearchTreeEntry(new GUIContent("Comment Block", _indentationIcon))
			{
				level = 1,
				userData = new Group()
			});

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            //Editor window-based mouse position
            var mousePosition = _window.rootVisualElement.ChangeCoordinatesTo(_window.rootVisualElement.parent,
                context.screenMousePosition - _window.position.position);
            var graphMousePosition = _graphView.contentViewContainer.WorldToLocal(mousePosition);

	        NoodlesNode newNode = SearchTreeEntry.userData as NoodlesNode;

			if (newNode!= null)
	        {
		        newNode.Set(graphMousePosition, Guid.NewGuid().ToString());
				_graphView.AddNodeToNoodle(newNode);
		        return true;
	        }

	        //switch (SearchTreeEntry.userData)
            //{
            //    case DialogueNode dialogueNode:
            //        _graphView.CreateNewDialogueNode("Dialogue Node",graphMousePosition);
            //        return true;
            //    case Group group:
            //        var rect = new Rect(graphMousePosition, _graphView.DefaultCommentBlockSize);
            //         _graphView.CreateCommentBlock(rect);
            //        return true;
            //}
            return false;
        }
    }
}