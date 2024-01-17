#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor
{
    public class GraphEditorPort : Port
    {
        private GraphEditorPort(Orientation portOrientation, Direction portDirection, Port.Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type) {}
 
        public new static Port Create<TEdge>(
            Orientation orientation,
            Direction direction,
            Port.Capacity capacity,
            Type type,
            GraphEditorView graphView)
            where TEdge : Edge, new()
        {
            DefaultEdgeConnectorListener listener = new DefaultEdgeConnectorListener(graphView);
            GraphEditorPort ele = new GraphEditorPort(orientation, direction, capacity, type)
            {
                m_EdgeConnector = new EdgeConnector<TEdge>(listener)
            };
            ele.AddManipulator(ele.m_EdgeConnector);
            return ele;
        }
 
        private class DefaultEdgeConnectorListener : IEdgeConnectorListener
        {
            private GraphViewChange _graphViewChange;
            private List<Edge> _edgesToCreate;
            private List<GraphElement> _edgesToDelete;

            private GraphEditorView _graphView;
 
            public DefaultEdgeConnectorListener(GraphEditorView graphView)
            {
                _edgesToCreate = new List<Edge>();
                _edgesToDelete = new List<GraphElement>();
                _graphViewChange.edgesToCreate = _edgesToCreate;

                _graphView = graphView;
            }
 
            public void OnDropOutsidePort(Edge edge, Vector2 position)
            {
                _graphView.OnEdgeDropped(edge, position);
            }
 
            public void OnDrop(GraphView graphView, Edge edge)
            {
                _edgesToCreate.Clear();
                _edgesToCreate.Add(edge);
                _edgesToDelete.Clear();
                if (edge.input.capacity == Port.Capacity.Single)
                {
                    foreach (Edge connection in edge.input.connections)
                    {
                        if (connection != edge)
                            _edgesToDelete.Add(connection);
                    }
                }
 
                if (edge.output.capacity == Port.Capacity.Single)
                {
                    foreach (Edge connection in edge.output.connections)
                    {
                        if (connection != edge)
                            _edgesToDelete.Add(connection);
                    }
                }
 
                if (_edgesToDelete.Count > 0)
                    graphView.DeleteElements(_edgesToDelete);
                List<Edge> edgesToCreate = _edgesToCreate;
                if (graphView.graphViewChanged != null)
                    edgesToCreate = graphView.graphViewChanged(_graphViewChange).edgesToCreate;
                foreach (Edge edge1 in edgesToCreate)
                {
                    graphView.AddElement(edge1);
                    edge.input.Connect(edge1);
                    edge.output.Connect(edge1);
                }
            }
        }
    }
}
#endif