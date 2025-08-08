using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Sommerhåndbold
{
    class HandballBiz : INotifyPropertyChanged
    {
        public HandballBiz()
        {
            //model.Database.Initialize(false);
            model.Matches.Load();
        }

        private readonly HandballModel model = new HandballModel();
        public event PropertyChangedEventHandler PropertyChanged;
        private Random rnd = new Random(DateTime.Now.Second);

        public List<Team> Teams
        {
            get
            {
                // Load Teams but also seeds
                return model.Teams.Include(t => t.Seed).ToList();
            }
        }

        public List<Seed> Seeds
        {
            get
            {
                // Load Seeds but also teams
                return model.Seeds.Include(s => s.Teams).ToList();
            }
        }

        public Group Group1
        {
            get
            {
                if (model.Groups.Count() > 0)
                {
                    // Load group but also teams
                    return model.Groups.Include(g => g.Teams).Include(h => h.Teamscores).ToList()[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public Group Group2
        {
            get
            {
                if (model.Groups.Count() > 0)
                {
                    // Load group but also teams
                    return model.Groups.Include(g => g.Teams).Include(h => h.Teamscores).ToList()[1];
                }
                else
                {
                    return null;
                }
            }
        }
        public Group Group3
        {
            get
            {
                if (model.Groups.Count() > 0)
                {
                    // Load group but also teams
                    return model.Groups.Include(g => g.Teams).Include(h => h.Teamscores).ToList()[2];
                }
                else
                {
                    return null;
                }
            }
        }
        public Group Group4
        {
            get
            {
                if (model.Groups.Count() > 0)
                {
                    // Load group but also teams
                    return model.Groups.Include(g => g.Teams).Include(h => h.Teamscores).ToList()[3];
                }
                else
                {
                    return null;
                }
            }
        }

        public Intermediate Intermediate1
        {
            get
            {
                if (model.InterMediates.Count() > 0)
                {
                    // Load group but also teams
                    return model.InterMediates.Include(g => g.Teams).Include(h => h.Teamscores).ToList()[0];
                }
                else
                {
                    return null;
                }

            }
        }

        public Intermediate Intermediate2
        {
            get
            {
                if (model.InterMediates.Count() > 0)
                {
                    // Load group but also teams
                    return model.InterMediates.Include(g => g.Teams).Include(h => h.Teamscores).ToList()[1];
                }
                else
                {
                    return null;
                }

            }
        }

        public Final? GetFinal(int placement)
        {
            if (model.Finals.Local.Count() > 0)
            {
                Final fin = (from f in model.Finals.Local where f.Placement == placement select f).First();
                return (from f in model.Finals.Local where f.Placement == placement select f).First();
            }
            else
            {
                return null;
            }
        }

        public Final GoldFinal
        {
            get
            {
                return GetFinal(1);
            }
        }

        public Final BronzeFinal
        {
            get
            {
                return GetFinal(3);
            }
        }

        Group _SelectedGroup = null;
        public Group SelectedGroup
        {
            get
            {
                return _SelectedGroup;
            }
            set
            {
                _SelectedGroup = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedGroup)));
            }
        }

        private Match _SelectedMatch = null;
        public Match SelectedMatch
        {
            get
            {
                return _SelectedMatch;
            }
            set
            {
                _SelectedMatch = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedMatch)));
            }
        }

        // Make result, but do not save (yet)
        private void MakeResult(Match match, int home, int away)
        {
            match.Score1 = home;
            match.Score2 = away;
            match.Played = true;
        }

        public void EnterResult(int home, int away)
        {
            MakeResult(SelectedMatch, home, away);
            model.SaveChanges();
            SelectedMatch.SendPropertyChanged("ScoreFor");
            SelectedMatch.SendPropertyChanged("ScoreAgainst");
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Group1)));
            Group1.SendPropertyChanged("Matches");
        }

        public void OpretKampe()
        {
            OpretKampe(Group1);
            OpretKampe(Group2);
            OpretKampe(Group3);
            OpretKampe(Group4);

            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Group1)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Group2)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Group3)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Group4)));
        }

        private void OpretKampe(Group group)
        {
            group.Matches.Clear();
            TilføjKampe(group);
        }

        private void TilføjKampe(Group theGroup)
        {
            var teams = theGroup.Teams.ToList();

            int count = theGroup.Teams.Count;
            int add = 1;
            int second = 0;
            while (add < count)
            {
                int blockStart = 0;
                int first = 0;
                while (first < count)
                {
                    second = (first + add) % count;
                    // Create match if not already created
                    if ((from m in theGroup.Matches
                         where (m.Team1 == teams[second] && m.Team2 == teams[first]) ||
                         (m.Team1 == teams[first] && m.Team2 == teams[second])
                         select m).Count() == 0)
                    {
                        Match match = new Match()
                        {
                            Team1 = teams[first],
                            Team2 = teams[second],
                        };
                        AddMatchToGroup(match, theGroup);
                    }
                    NextFirst(ref first, second, add, ref blockStart, count);
                }
                add++;
            }
            model.Groups.Load();
        }

        private void AddMatchToGroup(Match match, Group group)
        {
            group.Matches.Add(match);
        }

        public void NextFirst(ref int first, int second, int add, ref int blockStart, int count)
        {
            first++;
            if (first == blockStart + add)
            {
                // Start new block
                blockStart = blockStart + add + 1;
                first = blockStart;
            }
        }

        public void SimulerResultater()
        {
            foreach (Match match in model.Matches)
            {
                if (!match.Played)
                {
                    SimulerResultat(match);
                }
            }
            model.SaveChanges();

            foreach (Group group in model.Groups)
            {
                group.SendPropertyChanged(nameof(group.TeamscoresOrdered));
            }

            PropertyChanged(this, new PropertyChangedEventArgs(nameof(GoldFinal)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(BronzeFinal)));

        }

        private void MakeInterMediateObjects()
        {
            // Remove all Intermediates
            model.InterMediates.RemoveRange(model.InterMediates);

            Intermediate intermediate = new Intermediate()
            {
                GroupName = "Mellemrunde 1",
            };
            model.InterMediates.Add(intermediate);
            intermediate = new Intermediate()
            {
                GroupName = "Mellemrunde 2",
            };
            model.InterMediates.Add(intermediate);
            model.SaveChanges();
        }

        private void RemoveFinals()
        {
            //foreach (Final f in model.Finals)
            //{
            //    foreach (Teamscore teamscore in f.Teamscores)
            //    {
            //        f.Teamscores.Remove(teamscore);
            //    }
            //    foreach (Team team in f.Teams)
            //    {
            //        f.Teams.Remove(team);
            //    }
            //}
            
            // Remove old finals
            model.Finals.RemoveRange(model.Finals);
            model.Finals.Load();

            PropertyChanged(this, new PropertyChangedEventArgs(nameof(GoldFinal)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(BronzeFinal)));
        }

        private void MakeFilnalObjects()
        {
            RemoveFinals();

            Final final = new Final()
            {
                Placement = 1,
                GroupName = "Finale",
            };
            model.Finals.Add(final);

            Final bronze = new Final()
            {
                Placement = 3,
                GroupName = "Bronzekamp",
            };
            model.Finals.Add(bronze);
            model.Finals.Load();
        }

        private void Populate(Intermediate intermediate, Group group)
        {
            int i = 0;
            foreach (Teamscore score in group.TeamscoresOrdered)
            {
                i++;
                if (i > 3)
                {
                    break;
                }
                AddTeamToGroup(score.Team, intermediate);
            }
        }

        private void Populate(Final final, Group group)
        {
            if (final.Placement == 1)
            {
                AddTeamToGroup(group.TeamscoresOrdered.ElementAt(0).Team, final);
            }
            else
            {
                AddTeamToGroup(group.TeamscoresOrdered.ElementAt(1).Team, final);
            }
        }

        private void PopulateIntermediateTeams()
        {
            Populate(Intermediate1, Group1);
            Populate(Intermediate1, Group2);
            Populate(Intermediate2, Group3);
            Populate(Intermediate2, Group4);
        }

        private void PopulateFinalTeams()
        {
            Populate(GoldFinal, Intermediate1);
            Populate(BronzeFinal, Intermediate1);
            Populate(GoldFinal, Intermediate2);
            Populate(BronzeFinal, Intermediate2);
        }

        private void TransferIntermediateMatches(Intermediate intermediate, Group group)
        {
            foreach (Match match in group.Matches)
            {
                if (intermediate.Teams.Contains(match.Team1) &&
                    intermediate.Teams.Contains(match.Team2))
                {
                    // Copy match or else it will be removed from original group
                    intermediate.Matches.Add(new Match(match));
                }
            }
        }

        private void TransferIntermediateMatches()
        {
            TransferIntermediateMatches(Intermediate1, Group1);
            TransferIntermediateMatches(Intermediate1, Group2);
            TransferIntermediateMatches(Intermediate2, Group3);
            TransferIntermediateMatches(Intermediate2, Group4);
        }

        private void CreateIntermediateMatches()
        {
            TilføjKampe(Intermediate1);
            TilføjKampe(Intermediate2);
        }

        private void MakeIntermediateMatches()
        {
            TransferIntermediateMatches();
            CreateIntermediateMatches();
        }

        private void MakeFinalMatches()
        {
            TilføjKampe(GoldFinal);
            TilføjKampe(BronzeFinal);
        }

        public void MakeIntermediate()
        {
            MakeInterMediateObjects();
            PopulateIntermediateTeams();
            MakeIntermediateMatches();

            model.SaveChanges();

            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Intermediate1)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Intermediate2)));
        }

        public void MakeFinals()
        {
            MakeFilnalObjects();
            PopulateFinalTeams();
            MakeFinalMatches();

            model.SaveChanges();


            PropertyChanged(this, new PropertyChangedEventArgs(nameof(GoldFinal)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(BronzeFinal)));
        }

        public void SimulerResultat(Match match)
        {
            match.Score1 = rnd.Next(24, 34) - match.Team1.Seed.Layer;
            match.Score2 = rnd.Next(24, 34) - match.Team2.Seed.Layer;
            match.Played = true;
        }

        public void CreateGroups(List<List<string>> seeds)
        {
            CreateSeeds(seeds);
            CreateGroups();
        }

        private void RemoveAllTeamsAndGroups()
        {
            foreach (Group group in model.Groups)
            {
                group.Teamscores.Clear();
            }


            // Remove all result information
            model.Teamscores.RemoveRange(model.Teamscores);
            model.Teamscores.Load();

            // Remove all existing Matches.
            model.Matches.RemoveRange(model.Matches);
            model.Matches.Load();

            // Remove all existing Groups.
            model.Groups.RemoveRange(model.Groups);
            model.Groups.Load();

            // Remove all Intermediates
            model.InterMediates.RemoveRange(model.InterMediates);
            model.InterMediates.Load();

            // Remove all Finals
            RemoveFinals();

            // Remove all existing Teams
            model.Teams.RemoveRange(model.Teams);
            model.Teams.Load();

            // Remove all existing Seeds.
            model.Seeds.RemoveRange(model.Seeds);
            model.Seeds.Load();

            model.SaveChanges();
        }

        // Create new seeds
        // Delete all existing data.
        private void CreateSeeds(List<List<string>> seeds)
        {
            RemoveAllTeamsAndGroups();

            // Convert string lists to seeds. Add to database.
            int layer = 1;
            foreach (List<string> seedStrings in seeds)
            {
                Seed seed = new Seed(layer);
                foreach (string teamName in seedStrings)
                {
                    seed.Teams.Add(new Team(teamName));
                }
                model.Seeds.Add(seed);
                layer++;
            }
            model.SaveChanges();
        }

        private void AddTeamToGroup(Team team, Group group)
        {
            group.Teams.Add(team);
            Teamscore teamscore = new Teamscore()
            {
                Group = group,
                Team = team,
            };
            group.Teamscores.Add(teamscore);
        }

        // Destroy existing groups and create new groups
        //
        // First four : seeding group 1
        // Next four  : seeding group 2
        // -    -     : -       -     3
        // -    -     : -       -     4
        private void CreateGroups()
        {
            // Delete all existing groups
            model.Groups.RemoveRange(model.Groups);
            int next;
            const int LayerSize = 4;

            // Seed one
            List<Group> groups = new List<Group>();
            groups.Add(new Group() { GroupName = "Pulje A" });
            groups.Add(new Group() { GroupName = "Pulje B" });
            groups.Add(new Group() { GroupName = "Pulje C" });
            groups.Add(new Group() { GroupName = "Pulje D" });

            foreach (Seed seed in model.Seeds.ToList())
            {
                // Seed one
                int rest = LayerSize;
                List<Team> teams = seed.Teams.ToList();
                foreach (Group group in groups)
                {
                    next = rnd.Next(rest);
                    Team team = teams[next];
                    AddTeamToGroup(team, group);
                    teams.Remove(team);
                    rest--;
                }
            }

            // Insert all new groups in data model.
            model.Groups.AddRange(groups);
            model.SaveChanges();

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Group1"));
                PropertyChanged(this, new PropertyChangedEventArgs("Group2"));
                PropertyChanged(this, new PropertyChangedEventArgs("Group3"));
                PropertyChanged(this, new PropertyChangedEventArgs("Group4"));
                PropertyChanged(this, new PropertyChangedEventArgs("Teams"));
            }
        }
    }
}
