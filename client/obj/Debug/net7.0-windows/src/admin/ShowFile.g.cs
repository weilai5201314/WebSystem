﻿#pragma checksum "..\..\..\..\..\src\admin\ShowFile.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5F5F36483261342D8DA17DCBB064DC839900D1B9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using client.admin;


namespace client.admin {
    
    
    /// <summary>
    /// ShowFile
    /// </summary>
    public partial class ShowFile : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 59 "..\..\..\..\..\src\admin\ShowFile.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid PermissionDataGrid;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\..\..\src\admin\ShowFile.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox FileComboBox;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\..\..\src\admin\ShowFile.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox UserComboBox;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/client;component/src/admin/showfile.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\src\admin\ShowFile.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.PermissionDataGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 2:
            this.FileComboBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.UserComboBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            
            #line 83 "..\..\..\..\..\src\admin\ShowFile.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_FlushAll);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 86 "..\..\..\..\..\src\admin\ShowFile.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_DeletePermission);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 90 "..\..\..\..\..\src\admin\ShowFile.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_AddReadPermission);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 94 "..\..\..\..\..\src\admin\ShowFile.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_AddWritePermission);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

