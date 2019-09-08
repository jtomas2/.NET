using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain.Event;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Sabio.Services
{
    public class EventStatusService : IEventStatusService
    {
        private IDataProvider _dataProvider = null;

        public EventStatusService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public List<EventStatus> Get()
        {
            List<EventStatus> list = null;

            string procName = "dbo.EventStatus_SelectAll";

            _dataProvider.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                EventStatus model = new EventStatus();
                int startingIndex = 0;

                model.Id = reader.GetSafeInt32(startingIndex++);
                model.Name = reader.GetSafeString(startingIndex++);

                if (list == null)
                {
                    list = new List<EventStatus>();
                }
                list.Add(model);
            });

            return list;
        }

    }
}
