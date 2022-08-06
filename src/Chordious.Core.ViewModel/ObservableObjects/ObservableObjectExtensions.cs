// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Reflection;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chordious.Core.ViewModel
{
    public static class ObservableObjectExtensions
    {
        public static void OnAllPropertiesChanged(this ObservableObject observableObject)
        {
            TypeInfo typeInfo = observableObject.GetType().GetTypeInfo();

            var onPropertyChangedMethod = typeInfo.GetMethod("OnPropertyChanged", BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (PropertyInfo propertyInfo in typeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                onPropertyChangedMethod.Invoke(observableObject, new object[] { propertyInfo.Name } );
            }
        }
    }
}
