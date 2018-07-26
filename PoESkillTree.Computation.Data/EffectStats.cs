﻿using System;
using System.Collections.Generic;
using System.Linq;
using EnumsNET;
using PoESkillTree.Computation.Common;
using PoESkillTree.Computation.Common.Builders;
using PoESkillTree.Computation.Common.Data;
using PoESkillTree.Computation.Data.Base;
using PoESkillTree.Computation.Data.Collections;

namespace PoESkillTree.Computation.Data
{
    /// <summary>
    /// <see cref="IGivenStats"/> implementation that provides the stats applied when effects are active.
    /// </summary>
    public class EffectStats : UsesStatBuilders, IGivenStats
    {
        private readonly Lazy<IReadOnlyList<GivenStatData>> _lazyGivenStats;

        public EffectStats(IBuilderFactories builderFactories)
            : base(builderFactories)
        {
            _lazyGivenStats = new Lazy<IReadOnlyList<GivenStatData>>(() => CreateCollection().ToList());
        }

        public IReadOnlyList<Entity> AffectedEntities { get; } = Enums.GetValues<Entity>().ToList();

        public IReadOnlyList<string> GivenStatLines { get; } = new string[0];

        public IReadOnlyList<GivenStatData> GivenStats => _lazyGivenStats.Value;

        private EffectStatCollection CreateCollection() => new EffectStatCollection
        {
            // ailments
            { Ailment.Shock, PercentIncrease, Damage.Taken, 50 },
            { Ailment.Chill, PercentReduce, Stat.AnimationSpeed, 30 },
            { Ailment.Freeze, PercentReduce, Stat.AnimationSpeed, 100 },
            // buffs
            { Buff.Fortify, PercentReduce, Damage.Taken.WithHits, 20 },
            { Buff.Maim, PercentReduce, Stat.MovementSpeed, 30 },
            { Buff.Intimidate, PercentIncrease, Damage.Taken, 10 },
            { Buff.Onslaught, PercentIncrease, Stat.CastSpeed, 20 },
            { Buff.Onslaught, PercentIncrease, Stat.MovementSpeed, 20 },
            {
                Buff.UnholyMight,
                BaseAdd, Physical.Damage.WithHitsAndAilments.GainAs(Chaos.Damage.WithHitsAndAilments), 30
            },
            { Buff.Conflux.Igniting, BaseSet, Ailment.Ignite.Source(AnyDamageType), 1 },
            { Buff.Conflux.Shocking, BaseSet, Ailment.Shock.Source(AnyDamageType), 1 },
            { Buff.Conflux.Chilling, BaseSet, Ailment.Chill.Source(AnyDamageType), 1 },
            { Buff.Conflux.Elemental, BaseSet, Ailment.Ignite.Source(AnyDamageType), 1 },
            { Buff.Conflux.Elemental, BaseSet, Ailment.Shock.Source(AnyDamageType), 1 },
            { Buff.Conflux.Elemental, BaseSet, Ailment.Chill.Source(AnyDamageType), 1 },
        };
    }
}