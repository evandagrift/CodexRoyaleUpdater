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
    class Program
    {
        static async Task Main(string[] args)
        {
            //Get Clan
            // Get all battles

            //members seen in last 20 min add to highpriority
            //other members add to main list
            //players within 20 min add to watch list

            while (true)
            {
                //check battles for players on watch
                //if time since last game > 20 removed from listd
            }
            Console.WriteLine();

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