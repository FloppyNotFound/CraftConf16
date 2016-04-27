using CraftConf16.Shared.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using System;

namespace CraftConf16.Shared
{
    public class App : Application
	{
        private Dictionary<string, List<Talk>> _allTalksDay1 { get; set; }
        private Dictionary<string, List<Talk>> _allTalksDay2 { get; set; }

        private int _amountTabs = 7;

        public List<TalkViewModel> TalkViewModels { get; set; }

        public App ()
		{
            var carouselPage = new TabbedPage();
            TalkViewModels = new List<TalkViewModel>();

            carouselPage.ToolbarItems.Add(CreateToolbar(ConfEvent.SessionDay1));
            carouselPage.ToolbarItems.Add(CreateToolbar(ConfEvent.SessionDay2));
            
            // Create ViewModels for all Pages (=Stages)
            for (int i = 1; i <_amountTabs; i++)
            {                
                var talkViewModel = new TalkViewModel();
                TalkViewModels.Add(talkViewModel);

                var stagePage = new Page1();
                stagePage.BindingContext = talkViewModel;

                carouselPage.Children.Add(stagePage);
            }

            MainPage = carouselPage;

            SetMainPage(ConfEvent.SessionDay1);
        }

        private ToolbarItem CreateToolbar(ConfEvent confEvent)
        {
            var toolbarItem = new ToolbarItem();

            switch (confEvent)
            {
                case ConfEvent.SessionDay1:
                    toolbarItem.Text = "Day 1";
                    toolbarItem.Clicked += delegate { SetMainPage(ConfEvent.SessionDay1); };
                    break;
                case ConfEvent.SessionDay2:
                    toolbarItem.Text = "Day 2";
                    toolbarItem.Clicked += delegate { SetMainPage(ConfEvent.SessionDay2); };
                    break;
                default:
                    throw new ArgumentException("Event not supported");
            }

            return toolbarItem;
        }

        private async void SetMainPage(ConfEvent sessionDay)
        {
            if(_allTalksDay1 == null || _allTalksDay2 == null)
                await LoadCalender();

            int numElements = 0;
            Dictionary<string, List<Talk>> talkDict;

            switch (sessionDay)
            {
                case ConfEvent.SessionDay1:
                    numElements = _allTalksDay1.Keys.Count();
                    talkDict = _allTalksDay1;
                    break;
                case ConfEvent.SessionDay2:
                    numElements = _allTalksDay2.Keys.Count();
                    talkDict = _allTalksDay2;
                    break;
                default:
                    throw new ArgumentException("Event not supported");
            }

            for (int i = 0; i < numElements -1; i++)
            {
                var currentPage = (MainPage as TabbedPage).Children.ElementAt(i);
                currentPage.Title = talkDict.Keys.ElementAt(i);
                TalkViewModels.ElementAt(i).Talks = talkDict.Values.ElementAt(i);
                TalkViewModels.ElementAt(i).Stage = talkDict.Keys.ElementAt(i);
            }
        }

        private async Task LoadCalender()
        {
            var parser = new CalenderParser();
            _allTalksDay1 = await parser.GetSchedule(ConfEvent.SessionDay1);
            _allTalksDay2 = await parser.GetSchedule(ConfEvent.SessionDay2);
        }

        protected override void OnStart ()
		{
            // Handle when your app starts
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
