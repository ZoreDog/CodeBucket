using System;
using UIKit;
using CodeBucket.DialogElements;
using CodeBucket.Core.ViewModels.Issues;
using ReactiveUI;
using System.Reactive.Linq;
using System.Linq;

namespace CodeBucket.ViewControllers.Issues
{
    public class IssueVersionsViewController : DialogViewController
	{
        private IssueVersionsViewModel ViewModel { get; }

        public IssueVersionsViewController(IssueVersionsViewModel viewModel)
            : base(UITableViewStyle.Plain)
        {
            ViewModel = viewModel;
            Title = "Versions";
            EnableSearch = false;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            OnActivation(disposable =>
            {
                ViewModel
                    .Versions.Changed
                    .Select(_ => ViewModel.Versions.Select(CreateElement))
                    .Subscribe(x => Root.Reset(new Section { x }))
                    .AddTo(disposable);

                ViewModel
                    .LoadCommand
                    .IsExecuting
                    .Subscribe(x => TableView.IsLoading = x)
                    .AddTo(disposable);
                
                ViewModel
                    .DismissCommand
                    .Subscribe(_ => NavigationController.PopViewController(true))
                    .AddTo(disposable);
            });

            ViewModel.LoadCommand.ExecuteIfCan();
        }

        private static CheckElement CreateElement(IssueAttributeItemViewModel attribute)
        {
            var element = new CheckElement(attribute.Name);
            attribute.WhenAnyValue(x => x.IsSelected).Subscribe(x => element.Checked = x);
            element.CheckedChanged.InvokeCommand(attribute.SelectCommand);
            return element;
        }
	}
}

