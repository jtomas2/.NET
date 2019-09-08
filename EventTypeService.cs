using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain.Event;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Sabio.Services
{
    public class EventTypeService : IEventTypeService
    {
        private IDataProvider _dataProvider = null;

        public EventTypeService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public List<EventType> Get()
        {
            List<EventType> list = null;

            string procName = "dbo.EventTypes_SelectAll";

            _dataProvider.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                EventType model = new EventType();
                int startingIndex = 0;

                model.Id = reader.GetSafeInt32(startingIndex++);
                model.Name = reader.GetSafeString(startingIndex++);

                if (list == null)
                {
                    list = new List<EventType>();
                }
                list.Add(model);
            });

            return list;
        }
    }
}
