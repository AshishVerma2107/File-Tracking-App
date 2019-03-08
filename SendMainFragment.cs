using System;
using System.Collections.Generic;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;

namespace File_Tracking.Fragments
{
    public class SendMainFragment : Fragment
    {
        ViewPager viewpager;
        TabLayout tabLayout;
        Adapter fadapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.category, null);
            viewpager = view.FindViewById<ViewPager>(Resource.Id.viewpager);
            setupViewPager(viewpager);
            tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabs);
            tabLayout.SetupWithViewPager(viewpager);


            return view;

           
        }
        void setupViewPager(Android.Support.V4.View.ViewPager viewPager)
        {
            fadapter = new Adapter(Activity, ChildFragmentManager);
            //fadapter = new Adapter(SupportFragmentManager);
            fadapter.AddFragment(new ExistingLetter(FragmentManager), "Existing Letter");
            fadapter.AddFragment(new CreateNew(), "Create New");

            viewpager.Adapter = fadapter;
            viewpager.SetCurrentItem(0, true);
            viewpager.Adapter.NotifyDataSetChanged();
        }
        class Adapter : FragmentPagerAdapter
        {
            List<Android.Support.V4.App.Fragment> fragments = new List<Android.Support.V4.App.Fragment>();
            List<string> fragmentTitles = new List<string>();
            public Adapter(FragmentActivity activity, Android.Support.V4.App.FragmentManager fm) : base(fm) { }
            public void AddFragment(Android.Support.V4.App.Fragment fragment, String title)
            {
                fragments.Add(fragment);
                fragmentTitles.Add(title);
            }
            public override Android.Support.V4.App.Fragment GetItem(int position)
            {
                return fragments[position];
            }
            public override int Count
            {
                get
                {
                    return fragments.Count;
                }
            }
            public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
            {
                return new Java.Lang.String(fragmentTitles[position]);
            }
        }


    }
}
    
