namespace Sommerhåndbold
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;


    public class HandballModel : DbContext
    {
        // Your context has been configured to use a 'HandballModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Sommerhåndbold.HandballModel' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'HandballModel' 
        // connection string in the application configuration file.
        public HandballModel()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SommerHåndbold; Trusted_Connection=True; ");
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Group> Groups { get; set; }

        public virtual DbSet<Match> Matches { get; set; }

        public virtual DbSet<Seed> Seeds { get; set; }

        public virtual DbSet<Teamscore> Teamscores { get; set; }

        public virtual DbSet<Intermediate> InterMediates { get; set; }

        // Finals are two groups, each with two teams (meaning only one match)
        public virtual DbSet<Final> Finals { get; set; }
    }

    public class Team
    {
        public Team() { }
        public Team(string name)
        {
            TeamName = name;
        }
        public int Id { get; set; }
        public string TeamName { get; set; }

        [DeleteBehavior(DeleteBehavior.Restrict)]
        public Group? Group { get; set; }

        [DeleteBehavior(DeleteBehavior.Restrict)]
        public Seed Seed { get; set; }

        public override string ToString()
        {
            return TeamName;
        }
    }

    public class Seed
    {
        public Seed()
        {
            InitSeed();
        }
        public Seed(int layer)
        {
            Layer = layer;
            InitSeed();
        }
        public int Id { get; set; }
        public int Layer { get; set; }

        public ICollection<Team> Teams { get; set; }

        private void InitSeed()
        {
            Teams = new HashSet<Team>();
        }
    }

    public class Group : INotifyPropertyChanged
    {
        public int Id { get; set; }

        public string GroupName { get; set; }

        public ICollection<Team> Teams { get; set; } = new HashSet<Team>();

        public ICollection<Match> Matches { get; set; } = new HashSet<Match>();

        public ICollection<Teamscore> Teamscores { get; set; } = new HashSet<Teamscore>();

        [NotMapped]
        public IEnumerable<Teamscore> TeamscoresOrdered
        {
            get
            {
                return from score in Teamscores
                       orderby score.Points descending, score.ScoreDiff descending, score.ScoreFor descending
                       select score;
            }
        }

        [NotMapped]
        public IEnumerable<Match> MatchesOrdered
        {
            get
            {
                return from match in Matches
                       orderby match.Id ascending
                       select match;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SendPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return GroupName;
        }

    }

    public class Intermediate : Group
    {

    }

    public class Final : Group
    {
        public int Placement { get; set; }

        public Match TheMatch
        {
            get
            {
                if (Matches.Count == 0)
                    return null;

                return Matches.First();
            }
        }
    }

    public class Match : INotifyPropertyChanged
    {
        public int Id { get; set; }

        [DeleteBehavior(DeleteBehavior.Restrict)]
        public Team Team1 { get; set; }
        public int Score1 { get; set; }

        [DeleteBehavior(DeleteBehavior.Restrict)]
        public Team Team2 { get; set; }
        public int Score2 { get; set; }
        public bool Played { get; set; }

        public Match() { }

        public Match(Match match)
        {
            Played = match.Played;
            Score1 = match.Score1;
            Score2 = match.Score2;
            Team1 = match.Team1;
            Team2 = match.Team2;
        }

        public Match(Team team1, int score1, Team team2, int score2, bool played)
        {
            Team1 = team1;
            Score1 = score1;
            Team2 = team2;
            Score2 = score2;
            Played = played;
        }

        public Match(Team team1, Team team2)
        {
            Team1 = team1;
            Team2 = team2;
            Played = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Point(Team team)
        {
            if (team == Team1)
            {
                if (Score1 > Score2)
                    return 2;
                else if (Score1 == Score2)
                    return 1;
                else
                    return 0;
            }
            if (team == Team2)
            {
                if (Score2 > Score1)
                    return 2;
                else if (Score1 == Score2)
                    return 1;
                else
                    return 0;
            }
            return 0;
        }

        public void SendPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public int ScoreFor(Team team)
        {
            if (team == Team1)
            {
                return Score1;
            }
            if (team == Team2)
            {
                return Score2;
            }
            return 0;
        }

        public int ScoreAgainst(Team team)
        {
            if (team == Team1)
            {
                return Score2;
            }
            if (team == Team2)
            {
                return Score1;
            }
            return 0;
        }

    }


    public class Teamscore
    {
        public int Id { get; set; }

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Group Group { get; set; }

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Team Team { get; set; }

        [NotMapped]
        public IEnumerable<Match> Matches
        {
            get
            {
                if (Group == null)
                {
                    return Enumerable.Empty<Match>();
                }

                return from match in Group.Matches
                       where (match.Team1 == Team || match.Team2 == Team) && match.Played == true
                       select match;
            }
        }

        [NotMapped]
        public int ScoreDiff
        {
            get
            {
                return ScoreFor - ScoreAgainst;
            }
        }

        [NotMapped]
        public int ScoreFor
        {
            get
            {
                int goalsFor = 0;
                foreach (Match match in Matches)
                {
                    goalsFor += match.ScoreFor(Team);
                }
                return goalsFor;
            }
        }

        [NotMapped]
        public int ScoreAgainst
        {
            get
            {
                int goalsAgainst = 0;
                foreach (Match match in Matches)
                {
                    goalsAgainst += match.ScoreAgainst(Team);
                }
                return goalsAgainst;
            }
        }

        [NotMapped]
        public int Points
        {
            get
            {
                int points = 0;
                foreach (Match match in Matches)
                {
                    points += match.Point(Team);
                }
                return points;
            }
        }

    }
}