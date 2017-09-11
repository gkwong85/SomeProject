namespace PasswordService
{
    public void ChangePassword(User_ChangePasswordRequest userModel)
    {
        string salt;
        string passwordHash;
        string password = userModel.Password;
        byte[] hash = HashToken(userModel.Token);

        salt = _cryptographyService.GenerateRandomString(RAND_LENGTH);
        passwordHash = _cryptographyService.Hash(password, salt, HASH_ITERATION_COUNT);

        _dataProvider.ExecuteNonQuery("some_procedure", inputParamMapper: delegate (SqlParameterCollection paramCollection)
        {
            paramCollection.AddWithValue("@variable", userModel.Id);
            paramCollection.AddWithValue("@variable", salt);
            paramCollection.AddWithValue("@variable", passwordHash);
            paramCollection.AddWithValue("@variable", hash);
        });

        _authenticationService.LogIn(new UserBase
        {
            Id = userModel.Id,
            Name = String.Empty,
            Roles = new string[0]
        });
    }

    public int? CheckUserToken(string token)
            {
                int? userId = null;
                byte[] hash = HashToken(token);

                _dataProvider.ExecuteCmd("some_procedure", inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@variable", hash);
                }
                , singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int? id = reader.GetSafeInt32Nullable(0);

                    if (id != null)
                    {
                        userId = id;
                    }

                });
                return userId;
            }

    private void InsertPasswordResetToken(User_PasswordResetTokenRequest userModel)
    {
        _dataProvider.ExecuteNonQuery("some_procedure", inputParamMapper: delegate (SqlParameterCollection paramCollection)
        {
            paramCollection.AddWithValue("@variable", userModel.UserId);
            paramCollection.AddWithValue("@variable", userModel.Token);
        });
    }

    private string GenerateSha1Hash(int id)
    {
        byte[] bytes = new byte[32];
        using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(bytes);
        }

        SHA1Managed hashstring = new SHA1Managed();
        byte[] hash = hashstring.ComputeHash(bytes);

        User_PasswordResetTokenRequest userInfo = new User_PasswordResetTokenRequest();
        userInfo.UserId = id;
        userInfo.Token = hash;
        InsertPasswordResetToken(userInfo);

        return Convert.ToBase64String(bytes);
    }

    private byte[] HashToken(string token)
    {
        byte[] convertByte = Convert.FromBase64String(token);
        SHA1Managed hashstring = new SHA1Managed();
        byte[] hash = hashstring.ComputeHash(convertByte);
        return hash;
    }
}