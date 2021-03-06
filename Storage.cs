﻿// Copyright (c) 2018 cloudcrate solutions UG (haftungsbeschraenkt)

using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Browser.Interop;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudcrate.AspNetCore.Blazor.Browser.Storage
{
    public abstract class StorageBase
    {
        private string _fullTypeName;

        protected internal StorageBase()
        {
            _fullTypeName = GetType().FullName;
        }

        public void Clear()
        {
            RegisteredFunction.Invoke<object>($"{_fullTypeName}.Clear");
        }

        public string GetItem(string key)
        {
            return RegisteredFunction.Invoke<string>($"{_fullTypeName}.GetItem", key);
        }

        public T GetItem<T>(string key)
        {
            var json = GetItem(key);
            return string.IsNullOrEmpty(json) ? default(T) : JsonUtil.Deserialize<T>(json);
        }

        public string Key(int index)
        {
            return RegisteredFunction.Invoke<string>($"{_fullTypeName}.Key", index);
        }

        public int Length
        {
            get
            {
                return RegisteredFunction.Invoke<int>($"{_fullTypeName}.Length");
            }
        }

        public void RemoveItem(string key)
        {
            RegisteredFunction.Invoke<object>($"{_fullTypeName}.RemoveItem", key);
        }

        public void SetItem(string key, string data)
        {
            RegisteredFunction.Invoke<object>($"{_fullTypeName}.SetItem", key, data);
        }

        public void SetItem(string key, object data)
        {
            SetItem(key, JsonUtil.Serialize(data));
        }

        public string this[string key]
        {
            get
            {
                return RegisteredFunction.Invoke<string>($"{_fullTypeName}.GetItemString", key);
            }
            set
            {
                RegisteredFunction.Invoke<object>($"{_fullTypeName}.SetItemString", key, value);
            }
        }

        public string this[int index]
        {
            get
            {
                return RegisteredFunction.Invoke<string>($"{_fullTypeName}.GetItemNumber", index);
            }
            set
            {
                RegisteredFunction.Invoke<object>($"{_fullTypeName}.SetItemNumber", index, value);
            }
        }
    }

    public sealed class LocalStorage : StorageBase
    {

    }

    public sealed class SessionStorage : StorageBase
    {

    }

    public static class ServiceCollectionExtensions
    {
        public static void AddStorage(this IServiceCollection col)
        {
            col.AddSingleton<LocalStorage>();
            col.AddSingleton<SessionStorage>();
        }
    }
}
