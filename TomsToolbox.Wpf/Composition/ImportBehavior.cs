﻿namespace TomsToolbox.Wpf.Composition
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Interactivity;

    using TomsToolbox.Core;
    using TomsToolbox.Desktop;

    /// <summary>
    /// A behavior to set a dependency property of the associated object to a value retrieved from the IOC. The default target property is the <see cref="FrameworkElement.DataContextProperty"/>.
    /// </summary>
    /// <seealso cref="System.Windows.Interactivity.Behavior{FrameworkElement}" />
    public class ImportBehavior : Behavior<FrameworkElement>
    {
        private INotifyChanged _tracker;
        private Type _memberType;
        private string _contractName;
        private DependencyProperty _targetProperty = FrameworkElement.DataContextProperty;

        /// <summary>
        /// Gets or sets the exported type of the object to provide.
        /// </summary>
        public Type MemberType
        {
            get
            {
                return _memberType;
            }
            set
            {
                _memberType = value;
                Update();
            }
        }

        /// <summary>
        /// Gets or sets the optional contract name of the exported object.
        /// </summary>
        public string ContractName
        {
            get
            {
                return _contractName;
            }
            set
            {
                _contractName = value;
                Update();
            }
        }

        /// <summary>
        /// Gets or sets the target property to set. The default is <see cref="FrameworkElement.DataContextProperty"/>.
        /// </summary>
        public DependencyProperty TargetProperty
        {
            get
            {
                return _targetProperty;
            }
            set
            {
                _targetProperty = value;
                Update();
            }
        }

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            base.OnAttached();

            _tracker = AssociatedObject.Track(ExportProviderLocator.ExportProviderProperty);
            _tracker.Changed += ExportProvider_Changed;

            Update();
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>
        /// Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            if (_tracker != null)
                _tracker.Changed -= ExportProvider_Changed;

            base.OnDetaching();
        }

        private void ExportProvider_Changed(object sender, EventArgs e)
        {
            Update();
        }

        private void Update()
        {
            var memberType = MemberType;
            var dependencyProperty = TargetProperty;

            if ((memberType == null) || (dependencyProperty == null))
                return;

            var frameworkElement = AssociatedObject;
            var exportProvider = frameworkElement?.TryGetExportProvider();

            if (exportProvider == null)
                return;

            var value = exportProvider.GetExports(memberType, null, ContractName)
                .Select(item => item.Value)
                .FirstOrDefault();

            frameworkElement.SetValue(dependencyProperty, value);
        }
    }
}