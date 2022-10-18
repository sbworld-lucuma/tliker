// Auto likes posts and runs on a schedule
// uses this twitter library: https://github.com/linvi/tweetinvi

public async Task Any(AutoLikeSchedule req)
        {

            var authDetails = AuthRepository.GetUserAuthDetails(req.Id);
            var provider = authDetails.FirstOrDefault(x => x.Provider == "twitter");
            if (provider == null) return;

            var userCredentials = new TwitterCredentials(
                AppSettings.GetString("oauth.twitter.ConsumerKey"),
                AppSettings.GetString("oauth.twitter.ConsumerSecret"),
                provider.AccessToken, provider.AccessTokenSecret);

            var userClient = new TwitterClient(userCredentials);
            var data = userClient.Timelines.GetMentionsTimelineIterator();
            int count = 0;
            while (!data.Completed && count < 1)
            {
                var page = await data.NextPageAsync();
                foreach (var tweet in page)
                {
                    if (!tweet.Favorited)
                    {
                        try
                        {
                            await tweet.FavoriteAsync();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }

                    var t= await userClient.TweetsV2.GetTweetAsync("");
                  
                }
                count++;
            }

        }
