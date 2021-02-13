using CodexRoyaleUpdater.Handlers;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CodexRoyaleUpdater
{
    //to do, make players last seen update with added battles
    //sort out old battles to get faster response
    //make there be a full sweep every 3 hours or so to ensure no missed data
    class Program
    {
        static Task Main(string[] args)
        {
            Client client = new Client();

            ClansHandler clansHandler = new ClansHandler(client);

            PlayersHandler playersHandler = new PlayersHandler(client);
            BattlesHandler battlesHandler = new BattlesHandler(client);

            Clan weAreFunny;

            string clanTag = "#8CYPL8R";
            string randomClanTag = "#9QGPC82Y0";
            int initialCount = 0;
            int count = 0;


            //Get Clan
            // Get all battles

            weAreFunny = clansHandler.GetOfficialClan(clanTag).Result;

            clansHandler.AddClan(weAreFunny).Wait();


            weAreFunny.MemberList.ForEach(p =>
            {
                List<Battle> pBattles = battlesHandler.GetOfficialPlayerBattles(p.Tag).Result;

                count += battlesHandler.AddBattles(pBattles).Result;
            });



            Console.WriteLine("Initially added " + count + " battles.");

            //members seen in last 20 min add to highpriority
            //other members add to main list
            //players within 20 min add to watch list

            ///////
            //get intial count and log how much it's gone up since to make sure it's actively updating
            ///////////
            DateTime lastFullSearch = DateTime.UtcNow;
            DateTime lastFullSave = DateTime.UtcNow;
            while (true)
            {
                DateTime now = DateTime.UtcNow;
                List<Player> watchList = new List<Player>();
                TimeSpan timeSinceFullSearch = now - lastFullSearch;

                int x = 4;
                if (x > 3)
                {
                    weAreFunny = clansHandler.GetOfficialClan(clanTag).Result;

                    weAreFunny.MemberList.ForEach(p =>
                    {
                        //convert to int for comparison with player
                        //int playerDayInt = Int32.Parse(p.LastSeen.Substring(0, 8));
                        //only grabbing 4 characters because we only need up to the minute
                        //int playerTimeInt = Int32.Parse(p.LastSeen.Substring(8, 4));

                        //int daysSinceLoggin = dayInt - playerDayInt;

                        DateTime lastSeen = DateTime.ParseExact(p.LastSeen.Substring(0, 15), "yyyyMMddTHHmmss", CultureInfo.InvariantCulture);

                        //get the difference between last time player was seen and now
                        TimeSpan ts = now - lastSeen;

                        //if player was seen in the past 20 minutes they are added to the watch list if isn't already in list
                        if (ts.TotalMinutes < 30)
                        {
                            if (!watchList.Contains(p)) { watchList.Add(p); }
                        }
                    });
                }
                watchList.ForEach(p =>
                {
                    List<Battle> pBattles = battlesHandler.GetOfficialPlayerBattles(p.Tag).Result;

                    List<Battle> battlesToAdd = new List<Battle>();

                    pBattles.ForEach(b =>
                    {
                        DateTime timeOfBattle = DateTime.ParseExact(p.LastSeen.Substring(0, 15), "yyyyMMddTHHmmss", CultureInfo.InvariantCulture);

                        //get the difference between last time player was seen and now
                        TimeSpan alreadySaved = timeOfBattle - lastFullSave;
                        //if (alreadySaved.TotalSeconds > 0)
                        //{
                        //    Console.WriteLine();
                        //}
                        //else Console.WriteLine();
                    });


                    count = battlesHandler.AddBattles(pBattles).Result;
                    if (count > 0)
                        Console.WriteLine(p.Name + " played " + count + " games.");

                    //remove if sat too long
                });



            }

        }

    }
}

/*
while(true)
{
    var clans = await handler.GetClanData(clanTag, officialAPI);
    List<Player> players = clans.MemberList;

    players.ForEach(p =>
    {
        //20201210T040100.000Z
        //YYYYMMDDTHHMMSS
        string[] dateStringArray = p.LastSeen.Split(".");

        //Console.WriteLine(p.Name + ": " + dateStringArray[0]);
        Console.WriteLine(dateStringArray[1]);
        if(dateStringArray[1] != "000Z")
        {
            Console.WriteLine("Hold Up");
        }    

        //DateTime when = DateTime.ParseExact(p.LastSeen, "yyyyMMddTHHmmss", CultureInfo.InvariantCulture);
    });
}













/*



Player elodin = players.Where(p => p.Name == "Elodin").FirstOrDefault();
Player giggidy = players.Where(p => p.Name == "Giggidy").FirstOrDefault();

string elodinLastSeen = elodin.LastSeen;
Console.WriteLine("Elodin:" + elodinLastSeen);

string giggidyLastSeen = giggidy.LastSeen;
Console.WriteLine("Giggidy:" + giggidyLastSeen);

while (true)
{ 
    clans = await handler.GetClanData(clanTag, officialAPI);
    List<Player> updatedPlayers = clans.MemberList.OrderBy(p => p.Tag).ToList();

    for(int i = 0; i < updatedPlayers.Count;i++)
    {
        if(updatedPlayers[i].Tag == players[i].Tag)
        {
            if(updatedPlayers[i].LastSeen == players.)
        }
    }


    elodin = players.Where(p => p.Name == "Elodin").FirstOrDefault();

    if (elodinLastSeen != elodin.LastSeen)
    {
        elodinLastSeen = elodin.LastSeen;
        Console.WriteLine("Elodin:" + elodinLastSeen);
    }

    giggidy = players.Where(p => p.Name == "Giggidy").FirstOrDefault();
    if (giggidyLastSeen != giggidy.LastSeen)
    {
        giggidyLastSeen = giggidy.LastSeen;
        Console.WriteLine("Giggidy:" + giggidyLastSeen);
    }

}
*/