import React from 'react';
import { PlayerRanking } from '../types/football';
import { X, Shield, Swords, Zap, HeartHandshake } from 'lucide-react';

interface PlayerModalProps {
  player: PlayerRanking | null;
  onClose: () => void;
}

export const PlayerModal: React.FC<PlayerModalProps> = ({ player, onClose }) => {
  if (!player) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-slate-950/80 backdrop-blur-sm animate-fade-in">
      <div 
        className="relative w-full max-w-2xl bg-[#121826] border border-slate-700/80 rounded-3xl p-6 shadow-2xl overflow-hidden"
        onClick={(e) => e.stopPropagation()}
      >
        {/* Close Button */}
        <button
          onClick={onClose}
          className="absolute top-4 right-4 p-2 rounded-full bg-slate-800 text-slate-400 hover:text-white hover:bg-slate-700 transition-all"
        >
          <X className="w-4 h-4" />
        </button>

        {/* Player Header */}
        <div className="flex flex-col sm:flex-row items-center sm:items-start space-y-4 sm:space-y-0 sm:space-x-5 pb-6 border-b border-slate-800">
          <div className="w-20 h-20 rounded-2xl bg-slate-900 border border-slate-800 p-2 flex-shrink-0 shadow-lg relative">
            <img src={player.clubLogo} alt={player.clubName} className="w-full h-full object-contain" />
            <span className="absolute -bottom-2 -right-2 px-2 py-0.5 rounded-md bg-emerald-500 text-slate-950 text-[10px] font-black uppercase tracking-wider">
              {player.position}
            </span>
          </div>

          <div className="flex-1 text-center sm:text-left">
            <div className="flex flex-wrap items-center justify-center sm:justify-start gap-2 mb-1">
              <h2 className="text-xl font-black text-white tracking-tight">{player.name}</h2>
              <span className="text-xs px-2 py-0.5 bg-slate-800 text-slate-300 font-semibold rounded-md border border-slate-700">
                {player.nationality} ({player.countryCode})
              </span>
            </div>

            <p className="text-xs text-slate-400 mb-2">
              {player.clubName} • {player.competitionName} • {player.age} Years Old
            </p>

            <div className="flex flex-wrap items-center justify-center sm:justify-start gap-3 text-xs">
              <span className="text-slate-300">
                Market Value: <strong className="text-emerald-400">€{(player.marketValueEur / 1_000_000).toFixed(0)}M</strong>
              </span>
              <span className="text-slate-600">•</span>
              <span className="text-slate-300">
                Minutes: <strong className="text-white">{player.minutesPlayed}m</strong> ({player.matches} apps)
              </span>
            </div>
          </div>

          {/* Composite Power Rating Badge */}
          <div className="text-center bg-slate-900/90 border border-slate-800 p-3 rounded-2xl min-w-[90px]">
            <div className="text-2xl font-black text-emerald-400">
              {player.overallScore.toFixed(1)}
            </div>
            <div className="text-[10px] font-bold text-slate-400 uppercase tracking-wider">
              Power Score
            </div>
          </div>
        </div>

        {/* Algorithmic Subscore Pillars */}
        <div className="grid grid-cols-2 sm:grid-cols-4 gap-3 my-5">
          <div className="bg-slate-900/80 border border-slate-800/80 p-3 rounded-xl">
            <div className="flex items-center space-x-1.5 text-rose-400 mb-1">
              <Swords className="w-3.5 h-3.5" />
              <span className="text-[11px] font-bold">Attacking</span>
            </div>
            <div className="text-lg font-black text-white">{player.attackingScore.toFixed(1)}</div>
            <div className="w-full bg-slate-800 h-1.5 rounded-full mt-2 overflow-hidden">
              <div className="bg-rose-500 h-full rounded-full" style={{ width: `${player.attackingScore}%` }} />
            </div>
          </div>

          <div className="bg-slate-900/80 border border-slate-800/80 p-3 rounded-xl">
            <div className="flex items-center space-x-1.5 text-amber-400 mb-1">
              <Zap className="w-3.5 h-3.5" />
              <span className="text-[11px] font-bold">Playmaking</span>
            </div>
            <div className="text-lg font-black text-white">{player.playmakingScore.toFixed(1)}</div>
            <div className="w-full bg-slate-800 h-1.5 rounded-full mt-2 overflow-hidden">
              <div className="bg-amber-500 h-full rounded-full" style={{ width: `${player.playmakingScore}%` }} />
            </div>
          </div>

          <div className="bg-slate-900/80 border border-slate-800/80 p-3 rounded-xl">
            <div className="flex items-center space-x-1.5 text-blue-400 mb-1">
              <Shield className="w-3.5 h-3.5" />
              <span className="text-[11px] font-bold">Defending</span>
            </div>
            <div className="text-lg font-black text-white">{player.defendingScore.toFixed(1)}</div>
            <div className="w-full bg-slate-800 h-1.5 rounded-full mt-2 overflow-hidden">
              <div className="bg-blue-500 h-full rounded-full" style={{ width: `${player.defendingScore}%` }} />
            </div>
          </div>

          <div className="bg-slate-900/80 border border-slate-800/80 p-3 rounded-xl">
            <div className="flex items-center space-x-1.5 text-purple-400 mb-1">
              <HeartHandshake className="w-3.5 h-3.5" />
              <span className="text-[11px] font-bold">Discipline</span>
            </div>
            <div className="text-lg font-black text-white">{player.disciplineScore.toFixed(1)}</div>
            <div className="w-full bg-slate-800 h-1.5 rounded-full mt-2 overflow-hidden">
              <div className="bg-purple-500 h-full rounded-full" style={{ width: `${player.disciplineScore}%` }} />
            </div>
          </div>
        </div>

        {/* Per 90 Advanced Football Analytics */}
        <div>
          <h4 className="text-xs font-bold text-slate-400 uppercase tracking-wider mb-3">
            Per-90 Minute Statistical Profile
          </h4>
          <div className="grid grid-cols-3 sm:grid-cols-6 gap-2 text-center text-xs">
            <div className="bg-slate-900/60 p-2.5 rounded-xl border border-slate-800">
              <span className="text-[10px] text-slate-500 block">Goals / 90</span>
              <span className="font-extrabold text-white text-sm">{player.goalsPer90.toFixed(2)}</span>
            </div>
            <div className="bg-slate-900/60 p-2.5 rounded-xl border border-slate-800">
              <span className="text-[10px] text-slate-500 block">Assists / 90</span>
              <span className="font-extrabold text-emerald-400 text-sm">{player.assistsPer90.toFixed(2)}</span>
            </div>
            <div className="bg-slate-900/60 p-2.5 rounded-xl border border-slate-800">
              <span className="text-[10px] text-slate-500 block">xG / 90</span>
              <span className="font-extrabold text-rose-400 text-sm">{player.expectedGoalsPer90.toFixed(2)}</span>
            </div>
            <div className="bg-slate-900/60 p-2.5 rounded-xl border border-slate-800">
              <span className="text-[10px] text-slate-500 block">Key Pass / 90</span>
              <span className="font-extrabold text-amber-400 text-sm">{player.keyPassesPer90.toFixed(2)}</span>
            </div>
            <div className="bg-slate-900/60 p-2.5 rounded-xl border border-slate-800">
              <span className="text-[10px] text-slate-500 block">Tackles / 90</span>
              <span className="font-extrabold text-blue-400 text-sm">{player.tacklesPer90.toFixed(2)}</span>
            </div>
            <div className="bg-slate-900/60 p-2.5 rounded-xl border border-slate-800">
              <span className="text-[10px] text-slate-500 block">Pass Acc %</span>
              <span className="font-extrabold text-teal-400 text-sm">{player.passAccuracy.toFixed(1)}%</span>
            </div>
          </div>
        </div>

        {/* Algorithm Factors Footer */}
        <div className="mt-5 pt-4 border-t border-slate-800 flex flex-wrap items-center justify-between text-[11px] text-slate-500">
          <div>
            UEFA League Multiplier: <span className="text-slate-300 font-semibold">{player.leagueWeight}x</span>
          </div>
          <div>
            Sample Minutes Dampener: <span className="text-slate-300 font-semibold">{player.minutesDampener}</span>
          </div>
          <div>
            Avg Match Rating: <span className="text-slate-300 font-semibold">{player.rating.toFixed(2)} / 10</span>
          </div>
        </div>
      </div>
    </div>
  );
};
