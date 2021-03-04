﻿namespace Cosiness.Models.Common
{
    public abstract class BaseNamedEntity<TKey>
    {
        public TKey Id { get; set; }

        public string Name { get; set; }
    }
}