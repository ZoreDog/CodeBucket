using System;
using CodeBucket.DialogElements;
using CodeBucket.Core.ViewModels.Events;
using UIKit;
using CodeBucket.Core.Utils;
using CodeBucket.TableViewCells;
using BitbucketSharp.Models;
using System.Linq;
using System.Reactive.Linq;
using CodeBucket.Views;
using CodeBucket.TableViewSources;

namespace CodeBucket.ViewControllers.Events
{
    public abstract class BaseEventsViewController<TViewModel> : BaseViewController<TViewModel>
        where TViewModel : BaseEventsViewModel
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var tableView = new EnhancedTableView(UITableViewStyle.Plain)
            {
                ViewModel = ViewModel
            };

            this.AddTableView(tableView);
            var root = new RootElement(tableView);
            tableView.Source = new DialogElementTableViewSource(root);

            tableView.RegisterNibForCellReuse(NewsCellView.Nib, NewsCellView.Key);
            tableView.RowHeight = UITableView.AutomaticDimension;
            tableView.EstimatedRowHeight = 80f;

            var itemSection = new Section();
            root.Reset(itemSection);

            ViewModel.Items
                .ChangedObservable()
                .Subscribe(x => itemSection.Reset(x.Select(CreateElement)));

            //EndOfList.BindCommand(ViewModel.LoadMoreCommand);

            ViewModel.LoadMoreCommand.IsExecuting.Subscribe(x =>
            {
                if (x)
                {
                    var activity = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
                    activity.Frame = new CoreGraphics.CGRect(0, 0, 320, 64f);
                    activity.StartAnimating();
                    tableView.TableFooterView = activity;
                }
                else
                {
                    tableView.TableFooterView = null;
                }
            });
        }

        private static Element CreateElement(EventItemViewModel e)
        {
            try
            {
                var img = ChooseImage(e.EventType).ToImage();

				var headerBlocks = new System.Collections.Generic.List<NewsFeedElement.TextBlock>();
				foreach (var h in e.Header)
				{
					Action act = null;
					var anchorBlock = h as EventAnchorBlock;
					if (anchorBlock != null)
						act = anchorBlock.Tapped;
					headerBlocks.Add(new NewsFeedElement.TextBlock(h.Text, act));
				}

				var bodyBlocks = new System.Collections.Generic.List<NewsFeedElement.TextBlock>();
				foreach (var h in e.Body)
				{
					Action act = null;
					var anchorBlock = h as EventAnchorBlock;
					if (anchorBlock != null)
						act = anchorBlock.Tapped;
					var block = new NewsFeedElement.TextBlock(h.Text, act);
					bodyBlocks.Add(block);
				}

                return new NewsFeedElement(e.Avatar.ToUrl(), e.CreatedOn, headerBlocks, bodyBlocks, img, e.Tapped, e.Multilined);
            }
            catch
            {
                return null;
            }
        }

        private static AtlassianIcon ChooseImage(string eventName)
        {
			switch (eventName)
			{
				case EventModel.Type.ForkRepo:
                    return AtlassianIcon.Devtoolsrepositoryforked;
				case EventModel.Type.CreateRepo:
                    return AtlassianIcon.Devtoolsrepository;
				case EventModel.Type.Commit:
				case EventModel.Type.Pushed:
				case EventModel.Type.PullRequestFulfilled:
                    return AtlassianIcon.Devtoolscommit;
				case EventModel.Type.WikiUpdated:
				case EventModel.Type.WikiCreated:
				case EventModel.Type.PullRequestUpdated:
                    return AtlassianIcon.Edit;
                case EventModel.Type.WikiDeleted:
                case EventModel.Type.DeleteRepo:
                    return AtlassianIcon.Delete;
				case EventModel.Type.StartFollowUser:
				case EventModel.Type.StartFollowRepo:
				case EventModel.Type.StopFollowRepo:
				case EventModel.Type.StartFollowIssue:
				case EventModel.Type.StopFollowIssue:
                    return AtlassianIcon.Star;
				case EventModel.Type.IssueComment:
				case EventModel.Type.ChangeSetCommentCreated:
				case EventModel.Type.ChangeSetCommentDeleted:
				case EventModel.Type.ChangeSetCommentUpdated:
				case EventModel.Type.PullRequestCommentCreated:
				case EventModel.Type.PullRequestCommentUpdated:
				case EventModel.Type.PullRequestCommentDeleted:
                    return AtlassianIcon.Comment;
				case EventModel.Type.IssueUpdated:
				case EventModel.Type.IssueReported:
                    return AtlassianIcon.Flag;
				case EventModel.Type.ChangeSetLike:
				case EventModel.Type.PullRequestLike:
                    return AtlassianIcon.Like;
				case EventModel.Type.PullRequestUnlike:
				case EventModel.Type.PullRequestRejected:
				case EventModel.Type.ChangeSetUnlike:
                    return AtlassianIcon.Like;
                case EventModel.Type.PullRequestCreated:
                case EventModel.Type.PullRequestSuperseded:
                    return AtlassianIcon.Devtoolspullrequest;
                default:
                    return AtlassianIcon.Info;
            }
        }
    }
}