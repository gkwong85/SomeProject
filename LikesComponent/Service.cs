namespace LikesServices
{
    public class LikeService : ILikeService
    {
        private IDataProvider _dataProvider;

        public LikeService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public Dictionary<int, Like_GetLikesResponse> GetAll(Like_GetAllLikesByContentsAndUser model)
        {
            var results = new Dictionary<int, Like_GetLikesResponse>();

            _dataProvider.ExecuteCmd("some_procedure",                
                parameters =>
                {
                    var contentIds = parameters.AddWithValue("@variable", new IntIdTable(model.ContentIds));
                    var userId = parameters.AddWithValue("@variable", model.UserId);
                    contentIds.SqlDbType = System.Data.SqlDbType.Structured;
                    contentIds.TypeName = "dbo.IntIdTable";
                },
                (reader, set) =>
                {
                    var likeModel = new Like_GetLikesResponse();
                    var contentId = reader.GetInt32(0);
                    likeModel.Likes = reader.GetBoolean(1);
                    likeModel.LikesCount = reader.GetInt32(2);
                    results[contentId] = likeModel;
                });

            return results;
        }

        public void ToggleLike(Like_ModifyRequest model)
        {
            string procedure = "some_procedure";
            if (model.Like == true)
            {
                procedure = "some_procedure";
            }
            _dataProvider.ExecuteNonQuery(procedure, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@variable", model.ContentId);
                paramCollection.AddWithValue("@variable", model.UserId);
            });
        }
    }
}
