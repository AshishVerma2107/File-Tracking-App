using System;
using Plugin.Connectivity;

namespace File_Tracking
{
    class InternetConnection
    {
        public Boolean connectivity()
        {

            if (CrossConnectivity.Current.IsConnected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}