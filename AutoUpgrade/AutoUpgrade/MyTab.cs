using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AutoUpgrade
{
    [Activity(Label = "MyTab", MainLauncher = true)]
    public class MyTab : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            base.SetContentView(Resource.Layout.TT);
            this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            AddTab("tab1", new SimpleTabFragement("tab1"));
            AddTab("tab2", new SimpleTabFragement("tab2"));
            AddTab("tab3", new SimpleTabFragement("tab3"));
            AddTab("tab4", new SimpleTabFragement("tab4"));
            AddTab("tab5", new SimpleTabFragement("tab5"));
            AddTab("tab6", new SimpleTabFragement("tab6"));
            AddTab("tab7", new SimpleTabFragement("tab7"));
            AddTab("tab8", new SimpleTabFragement("tab8"));
            AddTab("tab9", new SimpleTabFragement("tab9"));
        }

        private void AddTab(string text, Fragment view)
        {
            var tab = this.ActionBar.NewTab();
            tab.SetText(text);

            tab.TabSelected += delegate (object sender, ActionBar.TabEventArgs e)
            {
                var fragment = this.FragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
                if (fragment != null)
                {
                    e.FragmentTransaction.Remove(fragment);
                }

                e.FragmentTransaction.Add(Resource.Id.fragmentContainer, view);
            };

            tab.TabUnselected += delegate (object sender, ActionBar.TabEventArgs e)
            {
                e.FragmentTransaction.Remove(view);
            };

            this.ActionBar.AddTab(tab);
        }
    }
}