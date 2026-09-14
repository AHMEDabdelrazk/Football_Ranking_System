import React from 'react';
import { ClubRanking } from '../types/football';
import { Shield, Award, Swords } from 'lucide-react';

interface ClubPowerRankingsProps {
  clubs: ClubRanking[];
  loading: boolean;
}

export const ClubPowerRankings: React.FC<ClubPowerRankingsProps> = ({ clubs, loading }) => {
  const getFormBadge = (result: string) => {
    switch (result.toUpperCase()) {
      case 'W':
        return 'bg-emerald-500/20 text-emerald-400 border-emerald-500/40';
      case 'D':
        return 'bg-amber-500/20 text-amber-400 border-amber-500/40';
      default:
        return 'bg-rose-500/20 text-rose-400 border-rose-500/40';
    }
  };

  return (
    <div className="space-y-4">
      {/* Header Info Banner */}
      <div className="bg-gradient-to-r from-emerald-900/30 via-slate-900/40 to-cyan-900/30 border border-emerald-500/20 rounded-2xl p-4 flex flex-col md:flex-row items-start md:items-center justify-between gap-3">
        <div className="flex items-center space-x-3">
          <div className="p-2 rounded-xl bg-emerald-500/10 text-emerald-400 border border-emerald-500/30">
            <Award className="w-6 h-6" />
          </div>
          <div>
            <h2 className="text-base font-bold text-white">European Club Power Index</h2>
            <p className="text-xs text-slate-400">
              Evaluates points percentage (35%), goal differential efficiency (25%), form momentum decay (20%), and squad depth rating (20%) normalized by UEFA coefficient.
            </p>
          </div>
        </div>
      </div>

      {/* Clubs Grid */}
      {loading ? (
        <div className="py-16 text-center text-slate-400">
          <div className="w-8 h-8 border-2 border-emerald-500 border-t-transparent rounded-full animate-spin mx-auto mb-2" />
          <span>Computing cross-league club power ratings...</span>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {clubs.map((club) => (
            <div
              key={club.clubId}
              className="bg-[#121826] border border-slate-800 hover:border-emerald-500/40 rounded-2xl p-4 shadow-lg hover:shadow-emerald-500/5 transition-all group flex flex-col justify-between"
            >
              <div>
                {/* Header: Rank + Logo + Name */}
                <div className="flex items-center justify-between mb-3">
                  <div className="flex items-center space-x-3">
                    <span className={`w-7 h-7 rounded-lg flex items-center justify-center font-black text-xs ${
                      club.rank === 1 ? 'bg-amber-400 text-slate-950 shadow-md shadow-amber-400/20' :
                      club.rank === 2 ? 'bg-slate-300 text-slate-950' :
                      club.rank === 3 ? 'bg-amber-700 text-white' :
                      'bg-slate-800 text-slate-300'
                    }`}>
                      #{club.rank}
                    </span>

                    <div className="w-10 h-10 rounded-xl bg-slate-900 border border-slate-800 p-1.5 flex-shrink-0">
                      <img src={club.logo} alt={club.name} className="w-full h-full object-contain" />
                    </div>

                    <div>
                      <h3 className="font-extrabold text-sm text-slate-100 group-hover:text-emerald-400 transition-colors">
                        {club.name}
                      </h3>
                      <span className="text-[10px] text-slate-400 bg-slate-800/60 px-1.5 py-0.5 rounded border border-slate-700/30">
                        {club.competitionName}
                      </span>
                    </div>
                  </div>

                  {/* Power Rating Gauge */}
                  <div className="text-right">
                    <div className="text-xl font-black bg-clip-text text-transparent bg-gradient-to-r from-emerald-400 to-teal-300">
                      {club.powerRating.toFixed(1)}
                    </div>
                    <div className="text-[10px] uppercase font-bold text-slate-500 tracking-wider">
                      Power Index
                    </div>
                  </div>
                </div>

                {/* Match Record Bar */}
                <div className="grid grid-cols-4 gap-2 bg-slate-900/80 p-2.5 rounded-xl border border-slate-800/80 text-center mb-3 text-xs">
                  <div>
                    <span className="text-[10px] text-slate-500 block">Record</span>
                    <span className="font-bold text-slate-200">{club.wins}-{club.draws}-{club.losses}</span>
                  </div>
                  <div>
                    <span className="text-[10px] text-slate-500 block">Goals</span>
                    <span className="font-bold text-slate-200">{club.goalsFor}:{club.goalsAgainst}</span>
                  </div>
                  <div>
                    <span className="text-[10px] text-slate-500 block">Diff</span>
                    <span className={`font-bold ${club.goalDifference >= 0 ? 'text-emerald-400' : 'text-rose-400'}`}>
                      {club.goalDifference > 0 ? `+${club.goalDifference}` : club.goalDifference}
                    </span>
                  </div>
                  <div>
                    <span className="text-[10px] text-slate-500 block">Points</span>
                    <span className="font-extrabold text-amber-400">{club.points}</span>
                  </div>
                </div>

                {/* Attack & Defense Bars */}
                <div className="space-y-2 mb-3">
                  <div>
                    <div className="flex justify-between text-[11px] font-semibold text-slate-400 mb-1">
                      <span className="flex items-center space-x-1">
                        <Swords className="w-3 h-3 text-rose-400" />
                        <span>Attack Rating</span>
                      </span>
                      <span className="text-rose-400 font-bold">{club.attackRating.toFixed(0)}</span>
                    </div>
                    <div className="w-full bg-slate-800 h-1.5 rounded-full overflow-hidden">
                      <div
                        className="bg-rose-500 h-full rounded-full transition-all duration-500"
                        style={{ width: `${club.attackRating}%` }}
                      />
                    </div>
                  </div>

                  <div>
                    <div className="flex justify-between text-[11px] font-semibold text-slate-400 mb-1">
                      <span className="flex items-center space-x-1">
                        <Shield className="w-3 h-3 text-blue-400" />
                        <span>Defense Rating</span>
                      </span>
                      <span className="text-blue-400 font-bold">{club.defenseRating.toFixed(0)}</span>
                    </div>
                    <div className="w-full bg-slate-800 h-1.5 rounded-full overflow-hidden">
                      <div
                        className="bg-blue-500 h-full rounded-full transition-all duration-500"
                        style={{ width: `${club.defenseRating}%` }}
                      />
                    </div>
                  </div>
                </div>
              </div>

              {/* Footer: Form Pills & Squad Score */}
              <div className="pt-2 border-t border-slate-800/80 flex items-center justify-between text-[11px]">
                <div className="flex items-center space-x-1">
                  <span className="text-slate-500 text-[10px] mr-1">Form:</span>
                  {club.recentForm.map((res, i) => (
                    <span
                      key={i}
                      className={`w-4 h-4 rounded text-[9px] font-black flex items-center justify-center border ${getFormBadge(res)}`}
                    >
                      {res}
                    </span>
                  ))}
                </div>

                <div className="text-slate-400 text-[10px]">
                  Squad Score: <span className="font-bold text-slate-200">{club.squadAverageScore.toFixed(1)}</span>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};
