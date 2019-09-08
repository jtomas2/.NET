using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.Event;
using Sabio.Models.Requests.Event;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sabio.Services
{
    public class EventService : IEventService
    {
        private IDataProvider _dataProvider = null;

        public EventService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public int Insert(EventAddRequest model, int userId)
        {
            int returnValue = 0;

            string procName = "[dbo].[Events_Insert]";

            _dataProvider.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@CreatedBy", userId);
                col.AddWithValue("@ModifiedBy", userId);
                AddCommonParams(model, col);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;
                col.Add(idOut);

            },
                returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@Id"].Value;
                    int.TryParse(oId.ToString(), out returnValue);
                });
            return returnValue;
        }


        public void Update(EventUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Events_Update]";
            _dataProvider.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@ModifiedBy", userId);
                    col.AddWithValue("@Id", model.Id);
                    AddCommonParams(model, col);
                },
                returnParameters: null);
        }


        public Event Get(int id)
        {
            string procName = "[dbo].[Events_Select_ById_v3]";
            
            Event model = null;

            _dataProvider.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", id);

                }, singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    model = MapReader(reader);

                });
                return model;
        }


        public Paged<Event> Get(int pageIndex, int pageSize)
        {
            Paged<Event> pagedResult = null;
            List<Event> result = null;
            int totalCount = 0;
            string procName = "[dbo].[Events_SelectAll_v2]";

            _dataProvider.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    Event aEvent = MapReader(reader);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(25);
                    }
                    if (result == null)
                    {
                        result = new List<Event>();
                    }

                    result.Add(aEvent);
                });

                if (result != null)
                {
                    pagedResult = new Paged<Event>(result, pageIndex, pageSize, totalCount);
                }
                return pagedResult;

        }


        public Paged<Event> Get(int pageIndex, int pageSize, int userId)
        {
            Paged<Event> pagedResult = null;
            List<Event> result = null;
            int totalCount = 0;
            string procName = "[dbo].[Events_Select_ByCreatedBy_v2]";

            _dataProvider.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);
                paramCollection.AddWithValue("@CreatedBy", userId);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                Event aEvent = MapReader(reader);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(25);
                }
                if (result == null)
                {
                    result = new List<Event>();
                }

                result.Add(aEvent);
            });

            if (result != null)
            {
                pagedResult = new Paged<Event>(result, pageIndex, pageSize, totalCount);
            }
            return pagedResult;

        }


        public void Delete(int id)
        {
            _dataProvider.ExecuteNonQuery("[dbo].[Events_Delete_ById]",
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);
                });
        }

        private static Event MapReader(IDataReader reader)
        {
            Event model = new Event();
            int startingIndex = 0;

            model.Id = reader.GetSafeInt32(startingIndex++);
            
            model.EventType = new EventType();
            model.EventType.Id = reader.GetSafeInt32(startingIndex++);
            model.EventType.Name = reader.GetSafeString(startingIndex++);

            model.Name = reader.GetSafeString(startingIndex++);
            model.Summary = reader.GetSafeString(startingIndex++);
            model.ShortDescription = reader.GetSafeString(startingIndex++);

            model.EventVenues = new EventVenues();
            model.EventVenues.Id = reader.GetSafeInt32(startingIndex++);
            model.EventVenues.Name = reader.GetSafeString(startingIndex++);

            model.EventVenues.EventLocation = new EventLocation();
            model.EventVenues.EventLocation.Id = reader.GetSafeInt32(startingIndex++);
            model.EventVenues.EventLocation.LineOne = reader.GetSafeString(startingIndex++);
            model.EventVenues.EventLocation.City = reader.GetSafeString(startingIndex++);
            model.EventVenues.EventLocation.Zip = reader.GetSafeString(startingIndex++);
            model.EventVenues.EventLocation.Latitude = reader.GetSafeDouble(startingIndex++);
            model.EventVenues.EventLocation.Longitude = reader.GetSafeDouble(startingIndex++);

            model.EventStatus = new EventStatus();
            model.EventStatus.Id = reader.GetSafeInt32(startingIndex++);
            model.EventStatus.Name = reader.GetSafeString(startingIndex++);

            model.ImageUrl = reader.GetSafeString(startingIndex++);
            model.ExternalSiteUrl = reader.GetSafeString(startingIndex++);
            model.IsFree = reader.GetSafeBool(startingIndex++);
            model.DateCreated = reader.GetSafeDateTime(startingIndex++);
            model.DateModified = reader.GetSafeDateTime(startingIndex++);
            model.DateStart = reader.GetSafeUtcDateTime(startingIndex++);
            model.DateEnd = reader.GetSafeUtcDateTime(startingIndex++);
            model.CreatedBy = reader.GetSafeInt32(startingIndex++);
            model.ModifiedBy = reader.GetSafeInt32(startingIndex++);
            return model;
        }

        private static void AddCommonParams(EventAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@EventTypeId", model.EventTypeId);
            col.AddWithValue("@Name", model.Name);
            col.AddWithValue("@Summary", model.Summary);
            col.AddWithValue("@ShortDescription", model.ShortDescription);
            col.AddWithValue("@VenueId", model.VenueId);
            col.AddWithValue("@EventStatusId", model.EventStatusId);
            col.AddWithValue("@ImageUrl", model.ImageUrl);
            col.AddWithValue("@ExternalSiteUrl", model.ExternalSiteUrl);
            col.AddWithValue("@IsFree", model.IsFree);
            col.AddWithValue("@DateStart", model.DateStart);
            col.AddWithValue("@DateEnd", model.DateEnd);
        }
    }
}
