﻿#pragma checksum "..\..\..\..\..\src\user\LogIn.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "AD195A2B121494F4911237A00A56C2847B274CE0"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
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


namespace client.user {
    
    
    /// <summary>
    /// LogIn
    /// </summary>
    public partial class LogIn : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 112 "..\..\..\..\..\src\user\LogIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Account;
        
        #line default
        #line hidden
        
        
        #line 120 "..\..\..\..\..\src\user\LogIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox Password;
        
        #line default
        #line hidden
        
        
        #line 122 "..\..\..\..\..\src\user\LogIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PasswordText;
        
        #line default
        #line hidden
        
        
        #line 124 "..\..\..\..\..\src\user\LogIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox ShowPasswordCheckBox;
        
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
            System.Uri resourceLocater = new System.Uri("/client;component/src/user/login.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\src\user\LogIn.xaml"
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
            this.Account = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.Password = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 121 "..\..\..\..\..\src\user\LogIn.xaml"
            this.Password.PasswordChanged += new System.Windows.RoutedEventHandler(this.Password_PasswordChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.PasswordText = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.ShowPasswordCheckBox = ((System.Windows.Controls.CheckBox)(target));
            
            #line 125 "..\..\..\..\..\src\user\LogIn.xaml"
            this.ShowPasswordCheckBox.Checked += new System.Windows.RoutedEventHandler(this.ShowPasswordCheckBox_Checked);
            
            #line default
            #line hidden
            
            #line 125 "..\..\..\..\..\src\user\LogIn.xaml"
            this.ShowPasswordCheckBox.Unchecked += new System.Windows.RoutedEventHandler(this.ShowPasswordCheckBox_Unchecked);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 131 "..\..\..\..\..\src\user\LogIn.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ToLogIn);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 134 "..\..\..\..\..\src\user\LogIn.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Jump_SignUp);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 136 "..\..\..\..\..\src\user\LogIn.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Jump_RevertPass);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
