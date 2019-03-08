using System;
using System.Collections.Generic;
using System.Linq;

using System;

using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using File_Tracking.Adapter;
using File_Tracking.Model;
using Newtonsoft.Json;
using System.Dynamic;
using System.Json;
using System.Threading.Tasks;
using Android.Preferences;

namespace File_Tracking.Fragments
{
    public class ReceiverListFragment : DialogFragment
    {
        ISharedPreferences prefs;
        Service restService;
        List<ReceiverDetails> result;
        ListView listView;
        SenderListAdapter adapter;
        string licenceid;
        string orgId;
        InternetConnection con;
        public FragmentManager fragManager;

        public ReceiverListFragment(FragmentManager fm)
        {
            this.fragManager = fm;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            con = new InternetConnection();
            
            prefs = PreferenceManager.GetDefaultSharedPreferences(Activity);
            
            restService = new Service();
           

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.SenderListView, container, false);
            

            listView = (ListView)view.FindViewById(Resource.Id.senderList);

           // dynamic value = new ExpandoObject();
           // value.QrDetail = search_string;
            //string json = JsonConvert.SerializeObject(value);

            if (con.connectivity())
            {
                 // FetchToFile().Wait();
                FetchToFile();
            }

            

            
            return view;

        }

        private async Task FetchToFile()
        {
            licenceid = prefs.GetString("LicenceId", "");
            orgId = prefs.GetString("OrganizationId", "");
            try
            {
                // JsonValue item = await restService.MarkingList(licenceid, orgId);

                JsonValue item = await restService.MarkingListFinal(licenceid, orgId).ConfigureAwait(false);
                result = JsonConvert.DeserializeObject<List<ReceiverDetails>>(item);
            }
            catch (Exception e)

            {
            }


            if (result != null)
            {
                Activity.RunOnUiThread(() =>
                {
                    adapter = new SenderListAdapter(Activity, result);
                    listView.SetAdapter(adapter);
                });
            }  
        }
    }
}