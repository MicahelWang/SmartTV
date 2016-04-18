using YeahTVApi.DomainModel.Models;
using YeahTVApi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Mapping
{
    public static class MapModelEntity
    {
        private static string httpHost;
        private static string hostName;

        public static List<ModelEntity> ToModelEntities(this List<ColumnMembersCacheModel> columnMembersCacheModels,
            LanguageType lang,
            string host,
            string hotelID,
            RequestHeader header)
        {
            hostName = host;
            httpHost = ChangetHttpsToHttps(hostName);
            var modelEntitys = new List<ModelEntity>();

            foreach (var c in columnMembersCacheModels)
            {
                modelEntitys.Add(new ModelEntity
                {
                    Action = c.TVmodelColumnItemAction.Contains("{hotelID}") ? 
                    c.TVmodelColumnItemAction.Replace("{hotelID}", hotelID).GetAction(c.TVmodelColumnItemActionType.Value) : 
                    c.TVmodelColumnItemAction.GetAction(c.TVmodelColumnItemActionType.Value),
                    
                    ActionType = c.TVmodelColumnItemActionType,
                    
                    BackgroundImageURL = lang==LanguageType.Chinese ?
                    string.IsNullOrEmpty(c.TVmodelColumnItemBackgroundImageUrl) ? string.Empty : httpHost + c.TVmodelColumnItemBackgroundImageUrl :
                    string.IsNullOrEmpty(c.TVmodelColumnItemEnBackgroundImageUrl) ? string.Empty : httpHost + c.TVmodelColumnItemEnBackgroundImageUrl,
                    
                    ColumnIndex = c.TVModelColumnColumnIndex.Value,

                    IconImageURL = string.IsNullOrEmpty(c.TVmodelColumnItemIconImageUrl) ? 
                                   string.Empty : 
                                   header.ScreenHeight>720? httpHost + c.TVmodelColumnItemIconImageUrl.Replace("{ScreenHeight}","icon_1080"):
                                                            httpHost + c.TVmodelColumnItemIconImageUrl.Replace("{ScreenHeight}","icon_720"),
                    
                    ModelCode = c.TVmodelColumnItemCode,
                    Title = lang == LanguageType.Chinese?c.TVmodelColumnItemTitle:c.TVmodelColumnItemEnTitle,
                    UseNetwork = c.TVmodelColumnItemUseNetwork.Value,
                    Weight = c.TVModelColumnMemberWeight.Value
                });
            }

            return modelEntitys;
        }

        private static string GetAction(this string action,int actionType)
        {
              
                if (action.Contains("{httpHost}"))
                    action = action.Replace("{httpHost}", httpHost);

                else if (action.IndexOf("{host}") > -1)
                    action = action.Replace("{host}", hostName);
            

            return action;
        }

        /// <summary>
        /// 将https的路径修改为http
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private static string ChangetHttpsToHttps(string host)
        {
            if (host.ToLower().IndexOf("https") > -1)
            {
                if (host.Split(':').Length == 2)
                {
                    host += ":443";
                }
                host = host.Replace(host.Substring(0, 5), "http").Replace(Constant.HttpsPort.ToString(), Constant.HttpPort.ToString());
            }
            return host;
        }
    }
}
