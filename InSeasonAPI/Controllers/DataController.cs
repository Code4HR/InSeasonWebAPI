﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InSeasonAPI.Models;
using Newtonsoft.Json;
using InSeasonAPI.Utils;

namespace InSeasonAPI.Controllers
{
    public class DataController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="animal"></param>
        /// <param name="date"></param>
        /// <param name="countyID"></param>
        /// <returns></returns>
        [Route("GetCounty/{animal}/{date}/{countyID}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Get(string animal, string date, string countyID)
        {
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(string.Format("~/App_Data/animals/{0}.json", animal))))
            {
                Hunting animalobj= await System.Threading.Tasks.Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Hunting>(reader.ReadToEnd()));
                List<List<Range>> seasons = animalobj.seasons.Select(x => x.range.Where(y => DateConverter.ConvertToDateTime(date)  >= DateConverter.ConvertToDateTime(y.season.date.starts) &&
                DateConverter.ConvertToDateTime(date) <= DateConverter.ConvertToDateTime(y.season.date.ends)).ToList()).ToList();

                var conversion = new Utils.Converter();
                var ids = conversion.CountyToGnis(Convert.ToInt32(countyID)).Select(x => x.FEATURE_ID).ToList();

                //List<int> gnissids = ids.Select(gnisCountyDefinition => gnisCountyDefinition.FEATURE_ID).ToList();

                if (seasons != null)
                {
                    var countiesRestrictions = seasons.Select(x => x.Where( y => ids.Contains(TypeConvert.StrToIntDef(y.places.Select(z => z.Value.gnis_id).FirstOrDefault(),0))));
                    var t = "n";

                    foreach (var item in countiesRestrictions)
                    {
                        if(item == null)
                        {
                   
                        }
                    }
                    // any place where the location == anything in ids
                    var ret = countiesRestrictions.Select(x => x.Where(y => y.places != null && x != null).Select(w => w.places.Select(z => z.Value)));
                    return Ok(ret);
                }
                return null;
            }
        }
    }
}
