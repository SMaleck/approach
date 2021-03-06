﻿using System;
using System.Collections.Generic;

namespace BehaviourTreeSystem
{
    public class BehaviourTreeIdGenerator
    {
        // ToDo Find a good way of making this unique and auto-generated
        private string _treeId = "T";

        private readonly Dictionary<Type, int> _nodeTypeCounts;

        public BehaviourTreeIdGenerator()
        {
            _nodeTypeCounts = new Dictionary<Type, int>();
        }

        public string GetId(Type nodeType)
        {
            if (!_nodeTypeCounts.ContainsKey(nodeType))
            {
                _nodeTypeCounts.Add(nodeType, 0);
            }

            var count = _nodeTypeCounts[nodeType];
            _nodeTypeCounts[nodeType]++;

            return $"{_treeId}_{nodeType.Name}_{count}";
        }
    }
}
