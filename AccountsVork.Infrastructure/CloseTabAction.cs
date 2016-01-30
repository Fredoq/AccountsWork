using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using Prism.Regions;

namespace AccountsWork.Infrastructure
{
    public class CloseTabAction : TriggerAction<Button>
    {
        protected override void Invoke(object parameter)
        {
            var args = parameter as RoutedEventArgs;
            if (args == null)
                return;
            var tabItem = FindParent<TabItem>(args.OriginalSource as DependencyObject);
            if (tabItem == null)
                return;
            var tabControl = FindParent<TabControl>(tabItem);
            if (tabControl == null)
                return;
            var region = RegionManager.GetObservableRegion(tabControl).Value;
            if (region == null)
                return;
            if (region.Views.Contains(tabItem.Content))
                region.Remove(tabItem.Content);
        }

        static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
                return null;
            var parent = parentObject as T;
            if (parent != null)
                return parent;
            return FindParent<T>(parentObject);
        }
    }
}