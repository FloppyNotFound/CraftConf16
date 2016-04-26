using CraftConf16.Shared.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

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
            var carouselPage = new CarouselPage();
            TalkViewModels = new List<TalkViewModel>();

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

            SetMainPage();
        }

        private async void SetMainPage()
        {
            await LoadCalender();          

            for (int i = 0; i < _allTalksDay1.Keys.Count() -1; i++)
            {
                var currentPage = (MainPage as CarouselPage).Children.ElementAt(i);
                currentPage.Title = _allTalksDay1.Keys.ElementAt(i);
                TalkViewModels.ElementAt(i).Talks = _allTalksDay1.Values.ElementAt(i);
                TalkViewModels.ElementAt(i).Stage = _allTalksDay1.Keys.ElementAt(i);
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
