using System;
using System.Collections.Generic;

namespace WSChat.Shared.Base
{
    public abstract class BaseManager<T>
    {
        protected List<T> ListData { get; private set; }

        protected BaseManager() =>
            ListData = new List<T>();

        protected void AddItem(T data)
        {
            if (ItemExists(data))
                throw new Exception("Item alredy exists");

            ListData.Add(data);
        }

        protected void RemoveItem(T item) => ListData.Remove(item);

        protected abstract bool ItemExists(T item);
    }
}
