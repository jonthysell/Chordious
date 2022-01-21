// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Reflection;

using GalaSoft.MvvmLight;

namespace Chordious.Core.ViewModel
{
    public static class ObservableObjectExtensions
    {
        public static void RaiseAllPropertiesChanged(this ObservableObject observableObject)
        {
            TypeInfo typeInfo = observableObject.GetType().GetTypeInfo();

            foreach (PropertyInfo propertyInfo in typeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                observableObject.RaisePropertyChanged(propertyInfo.Name);
            }
        }
    }
}
