using Exiled.API.Features;
using System.Collections.Generic;
using System;
using PlayerRoles;
using System.Linq;
using Exiled.Events.EventArgs.Player;
using Exiled.API.Enums;
using UnityEngine;
using Exiled.API.Features.Doors;
using AutoEvents.Features;

namespace AutoEvents.AutoEvents
{
    public class MtfVsCI : AutoEvent
    {
        public override string Name => "Мог vs Пх";

        public override string Description => "Мог против Пх. Игроки появляються в 3 случайных зон.";

        public override uint MinPlayers => 2;

        private Team _teamNtf { get; } = new Team("Мог", RoleTypeId.NtfCaptain);
        private Team _teamCi { get; } = new Team("Пх", RoleTypeId.ChaosMarauder);

        private RoleTypeId[] _ignoreRoles = new RoleTypeId[]
        {
            RoleTypeId.Overwatch, RoleTypeId.Tutorial
        };

        private uint _round = 0;
        public uint _maxRound = 5;
        private bool _roundRunning = false;
        private uint _roundTime = 240;

        private ZoneMaps[] Maps = new ZoneMaps[]
        {
            new ZoneMaps(ZoneType.HeavyContainment, 
                new RoomType[] { RoomType.HczEzCheckpointA, RoomType.HczEzCheckpointB}),

            new ZoneMaps(ZoneType.Entrance,
                new RoomType[] { RoomType.EzCheckpointHallwayA, RoomType.EzCheckpointHallwayB}),

        };

        private Kit[] _kits = new Kit[]
        {
            // Kit 1: Легкий
            new Kit(new ItemType[][]
            {
                // Категория 1: Пистолеты
                new ItemType[] { ItemType.GunCOM15, ItemType.Medkit, ItemType.ArmorLight, ItemType.Ammo9x19, ItemType.Ammo9x19, ItemType.Ammo9x19, ItemType.Ammo9x19, ItemType.Ammo9x19 },
        
                // Категория 2: Револьверы
                new ItemType[] { ItemType.GunRevolver, ItemType.Painkillers, ItemType.ArmorLight, ItemType.Ammo44cal, ItemType.Ammo44cal, ItemType.Ammo44cal, ItemType.Ammo44cal, ItemType.Ammo44cal },
        
                // Категория 3: Пистолеты-пулеметы
                new ItemType[] { ItemType.GunCrossvec, ItemType.Adrenaline, ItemType.ArmorCombat, ItemType.Ammo9x19, ItemType.Ammo9x19, ItemType.Ammo9x19, ItemType.Ammo9x19, ItemType.Ammo9x19 }
            }),
    
            // Kit 2: Штурмовой
            new Kit(new ItemType[][]
            {
                // Категория 1: Штурмовые винтовки
                new ItemType[] { ItemType.GunAK, ItemType.Medkit, ItemType.ArmorCombat, ItemType.Ammo762x39, ItemType.Ammo762x39, ItemType.Ammo762x39, ItemType.Ammo762x39, ItemType.Ammo762x39 },
        
                // Категория 2: Тактические винтовки
                new ItemType[] { ItemType.GunE11SR, ItemType.Painkillers, ItemType.ArmorCombat, ItemType.Ammo556x45, ItemType.Ammo556x45, ItemType.Ammo556x45, ItemType.Ammo556x45, ItemType.Ammo556x45 },
        
                // Категория 3: Пулеметы
                new ItemType[] { ItemType.GunLogicer, ItemType.Adrenaline, ItemType.ArmorHeavy, ItemType.Ammo762x39, ItemType.Ammo762x39, ItemType.Ammo762x39, ItemType.Ammo762x39, ItemType.Ammo762x39 }
            }),
    
            // Kit 3: Тактический
            new Kit(new ItemType[][]
            {
                // Категория 1: Дробовики
                new ItemType[] { ItemType.GunShotgun, ItemType.Medkit, ItemType.ArmorHeavy, ItemType.Ammo12gauge, ItemType.Ammo12gauge, ItemType.Ammo12gauge, ItemType.Ammo12gauge, ItemType.Ammo12gauge },
        
                // Категория 2: Снайперские
                new ItemType[] { ItemType.GunA7, ItemType.Painkillers, ItemType.ArmorCombat, ItemType.Ammo556x45, ItemType.Ammo556x45, ItemType.Ammo556x45, ItemType.Ammo556x45, ItemType.Ammo556x45 },
        
                // Категория 3: Специальное оружие
                new ItemType[] { ItemType.ParticleDisruptor, ItemType.Adrenaline, ItemType.ArmorHeavy, ItemType.Ammo44cal, ItemType.Ammo44cal, ItemType.Ammo44cal, ItemType.Ammo44cal, ItemType.Ammo44cal }
            }),
    
            // Kit 4: Специальный
            new Kit(new ItemType[][]
            {
                // Категория 1: Экспериментальное
                new ItemType[] { ItemType.GunFRMG0, ItemType.SCP500, ItemType.ArmorHeavy, ItemType.Ammo556x45, ItemType.Ammo556x45, ItemType.Ammo556x45, ItemType.Ammo556x45, ItemType.Ammo556x45 },
        
                // Категория 2: Скрытное
                new ItemType[] { ItemType.GunSCP127, ItemType.SCP207, ItemType.ArmorLight, ItemType.Ammo9x19, ItemType.Ammo9x19, ItemType.Ammo9x19, ItemType.Ammo9x19, ItemType.Ammo9x19 },
        
                // Категория 3: Тактическое
                new ItemType[] { ItemType.MicroHID, ItemType.Adrenaline, ItemType.ArmorCombat, ItemType.Ammo44cal, ItemType.Ammo44cal, ItemType.Ammo44cal, ItemType.Ammo44cal, ItemType.Ammo44cal }
            }),
    
            // Kit 5: Элитный
            new Kit(new ItemType[][]
            {
                // Категория 1: Штурм элитный
                new ItemType[] { ItemType.GunE11SR, ItemType.SCP500, ItemType.ArmorHeavy, ItemType.Ammo556x45, ItemType.Ammo556x45, ItemType.Ammo556x45, ItemType.Ammo556x45, ItemType.Ammo556x45, ItemType.Adrenaline },
        
                // Категория 2: Тактический элитный
                new ItemType[] { ItemType.GunAK, ItemType.Medkit, ItemType.ArmorHeavy, ItemType.Ammo762x39, ItemType.Ammo762x39, ItemType.Ammo762x39, ItemType.Ammo762x39, ItemType.Ammo762x39, ItemType.SCP207 },
        
                // Категория 3: Спецназ
                new ItemType[] { ItemType.GunFRMG0, ItemType.SCP500, ItemType.ArmorHeavy, ItemType.Ammo556x45, ItemType.Ammo556x45, ItemType.Ammo556x45, ItemType.Ammo556x45, ItemType.Ammo556x45, ItemType.Painkillers, ItemType.Adrenaline }
            })
        };

        private DateTime _roundTimeEnd;

        public override void Start()
        {
            RegisterEvents();
            StartRound();
            Server.FriendlyFire = false;
        }

        public override void Stop()
        {
            foreach (var player in Player.List)
            {
                player.Role.Set(RoleTypeId.Spectator);
            }

            UnregisterEvents();

            string teamWinner = (_teamNtf.WinCount > _teamCi.WinCount ? _teamNtf.Name : _teamCi.Name);

            Map.Broadcast(10,
                $"<b>Игра окончена. Победа {teamWinner}</b>");

            GC.SuppressFinalize(this);
        }

        protected override void Update()
        {
            if (!_roundRunning) return;

            if (DateTime.Now < _roundTimeEnd)
            {
                if (_teamCi.Players.Where(x => x.IsAlive).Count() <= 0 || _teamNtf.Players.Where(x => x.IsAlive).Count() <= 0)
                {
                    EndRound();
                }

            }
            else
            {
                EndRound();
            }


        }

        private void StartRound()
        {
            _roundRunning = true;
            _roundTimeEnd = DateTime.Now.AddSeconds(_roundTime);

            Map.CleanAllItems();
            Map.CleanAllRagdolls();
            Map.Clean(Decals.DecalPoolType.Blood, int.MaxValue);
            Map.Clean(Decals.DecalPoolType.Bullet, int.MaxValue);

            var map = Maps[UnityEngine.Random.Range(0, Maps.Length)];
            Door.LockAll(float.MaxValue, DoorLockType.Warhead);

            foreach (var d in Door.List.Where(x => x.Zone == map.ZoneType && !x.IsElevator))
            {
                d.IsOpen = true;
            }

            foreach (var player in Player.List
                .Where(x => !_teamCi.Players.Contains(x) && !_teamNtf.Players.Contains(x) && !_ignoreRoles.Contains(x.Role.Type)))
            {
                if (_teamCi.Players.Count >= _teamNtf.Players.Count)
                {
                    _teamNtf.Players.Add(player);
                }
                else
                {
                    _teamCi.Players.Add(player);
                }
            }

            SpawnPlayers(_teamCi, map.Spawns[0]);
            SpawnPlayers(_teamNtf, map.Spawns[1]);

            var kit = _kits[_round >= _kits.Length ? 0 : _round];
            var category = kit.Items[UnityEngine.Random.Range(0, kit.Items.Length)];

            foreach (var player in Player.List)
            {
                foreach (var item in category)
                {
                    player.AddItem(item);
                }
            }

            void SpawnPlayers(Team team, RoomType roomType)
            {
                foreach (var player in team.Players)
                {
                    player.Role.Set(team.Role, RoleSpawnFlags.None);
                    player.ClearInventory();
                    player.Teleport(roomType);
                }
            }
        }

        private void EndRound()
        {
            _roundRunning = false;
            _round++;

            int playerNtfAlive = _teamNtf.Players.Where(x => x.IsAlive).Count();
            int playerCiAlive = _teamCi.Players.Where(x => x.IsAlive).Count();

            if (playerCiAlive > playerNtfAlive)
            {
                _teamCi.WinCount++;
            }
            else if (playerNtfAlive > playerCiAlive)
            {
                _teamNtf.WinCount++;
            }

            string teamWinner = (playerNtfAlive > playerCiAlive ? _teamNtf.Name : _teamCi.Name);

            Map.ClearBroadcasts();
            Map.Broadcast(5,
                $"<b>Раунд окончен. Победа {teamWinner}</b>");

            if (_round >= _maxRound)
            {
                Map.ClearBroadcasts();
                Map.Broadcast(10,
                    $"<b>Игра окончена. Победа {(_teamNtf.WinCount > _teamCi.WinCount ? _teamNtf.Name : _teamCi.Name)}</b>");
                AutoEventManager.StopEvent();
            }
            else
            {
                StartRound();
            }
        }

        private void RegisterEvents()
        {
            Exiled.Events.Handlers.Player.Verified += JoinedPlayer;
            Exiled.Events.Handlers.Player.Left += LeftPlayer;
        }

        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.Verified -= JoinedPlayer;
            Exiled.Events.Handlers.Player.Left -= LeftPlayer;
        }

        private void JoinedPlayer(VerifiedEventArgs ev)
        {
            if (_teamCi.Players.Count > _teamNtf.Players.Count)
            {
                _teamNtf.Players.Add(ev.Player);
            }else
            {
                _teamCi.Players.Add(ev.Player);
            }
        }

        private void LeftPlayer(LeftEventArgs ev)
        {
            _teamNtf.Players.Remove(ev.Player);
            _teamCi.Players.Remove(ev.Player);
        }

        private class Team
        {
            public string Name { get; }
            public RoleTypeId Role { get; }
            public List<Player> Players { get; } = new List<Player>();

            public uint WinCount { get; set; } = 0;

            public Team(string name, RoleTypeId role)
            {
                Name = name;
                Role = role;
            }
        }

        private class ZoneMaps
        {
            public ZoneType ZoneType { get; }
            public RoomType[] Spawns { get; }

            public ZoneMaps(ZoneType zoneType, RoomType[] spawns)
            {
                ZoneType = zoneType;
                Spawns = spawns;
            }
        }

        private class Kit
        {
            public ItemType[][] Items { get; }

            public Kit(ItemType[][] items)
            {
                Items = items;
            }
        }
    }

    
}
