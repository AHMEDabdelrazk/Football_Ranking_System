import React from 'react';
import { PlayerRanking } from '../types/football';
import { Search, ArrowUpDown, ArrowUp, ArrowDown, Minus, Flame, Eye } from 'lucide-react';

interface PlayerLeaderboardProps {
  players: PlayerRanking[];
  loading: boolean;
  onSelectPlayer: (player: PlayerRanking) => void;
  searchQuery: string;
  setSearchQuery: (query: string) => void;
  positionFilter: string;
  setPositionFilter: (pos: string) => void;
  sortBy: string;
  setSortBy: (field: string) => void;
  sortDescending: boolean;
  setSortDescending: (desc: boolean) => void;
}

const POSITION_TABS = [
  { id: '', label: 'All Roles' },
  { id: 'FWD', label: 'Forwards (ST/LW/RW)' },
  { id: 'MID', label: 'Midfield (CAM/CM/CDM)' },
  { id: 'DEF', label: 'Defenders (CB/FB)' },
  { id: 'GK', label: 'Goalkeepers' },
];

export const PlayerLeaderboard: React.FC<PlayerLeaderboardProps> = ({
  players,
  loading,
  onSelectPlayer,
  searchQuery,
  setSearchQuery,
  positionFilter,
  setPositionFilter,
  sortBy,
  setSortBy,
  sortDescending,
  setSortDescending,
}) => {
  const handleSort = (column: string) => {
    if (sortBy === column) {
      setSortDescending(!sortDescending);
    } else {
      setSortBy(column);
      setSortDescending(true);
    }
  };

  const getPositionBadge = (pos: string) => {
    switch (pos) {
      case 'ST':
      case 'LW':
      case 'RW':
        return 'bg-rose-500/10 text-rose-400 border-rose-500/30';
      case 'CAM':
      case 'CM':
      case 'CDM':
        return 'bg-amber-500/10 text-amber-400 border-amber-500/30';
      case 'CB':
      case 'LB':
      case 'RB':
        return 'bg-blue-500/10 text-blue-400 border-blue-500/30';
      case 'GK':
        return 'bg-emerald-500/10 text-emerald-400 border-emerald-500/30';
      default:
        return 'bg-slate-500/10 text-slate-400 border-slate-500/30';
    }
  };

  const getScoreColor = (score: number) => {
    if (score >= 93) return 'from-amber-400 to-yellow-500 text-amber-300';
    if (score >= 90) return 'from-emerald-400 to-teal-500 text-emerald-300';
    if (score >= 85) return 'from-cyan-400 to-blue-500 text-cyan-300';
    if (score >= 80) return 'from-indigo-400 to-violet-500 text-indigo-300';
    return 'from-slate-400 to-slate-500 text-slate-300';
  };

  return (
    <div className="space-y-4">
      {/* Controls Bar */}
      <div className="flex flex-col md:flex-row items-stretch md:items-center justify-between gap-3 bg-[#121826] p-3 rounded-2xl border border-slate-800">
        {/* Role Filters */}
        <div className="flex items-center space-x-1 overflow-x-auto no-scrollbar">
          {POSITION_TABS.map((tab) => (
            <button
              key={tab.id}
              onClick={() => setPositionFilter(tab.id)}
              className={`px-3 py-1.5 rounded-xl text-xs font-semibold whitespace-nowrap transition-all ${
                positionFilter === tab.id
                  ? 'bg-emerald-500 text-slate-950 font-bold shadow-md shadow-emerald-500/20'
                  : 'text-slate-400 hover:text-slate-200 hover:bg-slate-800/60'
              }`}
            >
              {tab.label}
            </button>
          ))}
        </div>

        {/* Search Input */}
        <div className="relative min-w-[240px]">
          <Search className="w-4 h-4 absolute left-3 top-1/2 -translate-y-1/2 text-slate-400" />
          <input
            type="text"
            placeholder="Search player, club, nation..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            className="w-full pl-9 pr-4 py-1.5 bg-slate-900/80 border border-slate-700/60 rounded-xl text-xs text-slate-200 placeholder-slate-500 focus:outline-none focus:border-emerald-500 transition-colors"
          />
        </div>
      </div>

      {/* Players Table */}
      <div className="bg-[#121826] rounded-2xl border border-slate-800 overflow-hidden shadow-xl">
        <div className="overflow-x-auto">
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="border-b border-slate-800 bg-[#0e1420]/80 text-[11px] font-bold text-slate-400 uppercase tracking-wider">
                <th className="py-3.5 px-4 w-14 text-center cursor-pointer" onClick={() => handleSort('rank')}>
                  <div className="flex items-center justify-center space-x-1">
                    <span>#</span>
                    <ArrowUpDown className="w-3 h-3" />
                  </div>
                </th>
                <th className="py-3.5 px-4">Player & Club</th>
                <th className="py-3.5 px-3 text-center">Role</th>
                <th className="py-3.5 px-3 text-center">League</th>
                <th className="py-3.5 px-4 text-center cursor-pointer" onClick={() => handleSort('overallScore')}>
                  <div className="flex items-center justify-center space-x-1 text-emerald-400">
                    <Flame className="w-3.5 h-3.5" />
                    <span>Power Score</span>
                    <ArrowUpDown className="w-3 h-3" />
                  </div>
                </th>
                <th className="py-3.5 px-3 text-center cursor-pointer" onClick={() => handleSort('goals')}>
                  <div className="flex items-center justify-center space-x-1">
                    <span>G / A</span>
                    <ArrowUpDown className="w-3 h-3" />
                  </div>
                </th>
                <th className="py-3.5 px-3 text-center cursor-pointer" onClick={() => handleSort('attackingScore')}>
                  <div className="flex items-center justify-center space-x-1">
                    <span>Attack</span>
                    <ArrowUpDown className="w-3 h-3" />
                  </div>
                </th>
                <th className="py-3.5 px-3 text-center cursor-pointer" onClick={() => handleSort('playmakingScore')}>
                  <div className="flex items-center justify-center space-x-1">
                    <span>Playmake</span>
                    <ArrowUpDown className="w-3 h-3" />
                  </div>
                </th>
                <th className="py-3.5 px-3 text-center cursor-pointer" onClick={() => handleSort('defendingScore')}>
                  <div className="flex items-center justify-center space-x-1">
                    <span>Defense</span>
                    <ArrowUpDown className="w-3 h-3" />
                  </div>
                </th>
                <th className="py-3.5 px-4 text-right">Market Val</th>
                <th className="py-3.5 px-3 text-center">Action</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-800/60 text-xs">
              {loading ? (
                <tr>
                  <td colSpan={11} className="py-12 text-center text-slate-400">
                    <div className="flex flex-col items-center justify-center space-y-2">
                      <div className="w-8 h-8 border-2 border-emerald-500 border-t-transparent rounded-full animate-spin" />
                      <span>Calculating multi-factor rankings across top leagues...</span>
                    </div>
                  </td>
                </tr>
              ) : players.length === 0 ? (
                <tr>
                  <td colSpan={11} className="py-12 text-center text-slate-400">
                    No players found matching current filters.
                  </td>
                </tr>
              ) : (
                players.map((p) => (
                  <tr
                    key={p.playerId}
                    onClick={() => onSelectPlayer(p)}
                    className="hover:bg-slate-800/40 cursor-pointer transition-colors group"
                  >
                    {/* Rank & Change */}
                    <td className="py-3 px-4 text-center">
                      <div className="flex items-center justify-center space-x-1">
                        <span className={`font-extrabold text-sm ${p.rank <= 3 ? 'text-amber-400' : 'text-slate-300'}`}>
                          {p.rank}
                        </span>
                        {p.rankChange > 0 && (
                          <span className="flex items-center text-[10px] text-emerald-400">
                            <ArrowUp className="w-2.5 h-2.5" />
                          </span>
                        )}
                        {p.rankChange < 0 && (
                          <span className="flex items-center text-[10px] text-rose-400">
                            <ArrowDown className="w-2.5 h-2.5" />
                          </span>
                        )}
                        {p.rankChange === 0 && (
                          <span className="flex items-center text-[10px] text-slate-500">
                            <Minus className="w-2 h-2" />
                          </span>
                        )}
                      </div>
                    </td>

                    {/* Player Info */}
                    <td className="py-3 px-4">
                      <div className="flex items-center space-x-3">
                        <div className="w-9 h-9 rounded-full bg-slate-800 border border-slate-700/80 overflow-hidden flex-shrink-0 relative">
                          <img
                            src={p.clubLogo || `https://media.api-sports.io/football/teams/50.png`}
                            alt={p.clubName}
                            className="w-full h-full object-contain p-1"
                          />
                        </div>
                        <div>
                          <div className="font-bold text-slate-100 group-hover:text-emerald-400 transition-colors flex items-center space-x-1.5">
                            <span>{p.name}</span>
                            <span className="text-[10px] px-1.5 py-0.2 bg-slate-800 text-slate-400 rounded">
                              {p.countryCode}
                            </span>
                          </div>
                          <div className="text-[11px] text-slate-400 flex items-center space-x-2">
                            <span>{p.clubName}</span>
                            <span>•</span>
                            <span>{p.age} yrs</span>
                            <span>•</span>
                            <span>{p.matches} apps ({p.minutesPlayed}m)</span>
                          </div>
                        </div>
                      </div>
                    </td>

                    {/* Position */}
                    <td className="py-3 px-3 text-center">
                      <span className={`px-2 py-0.5 text-[11px] font-bold rounded-lg border ${getPositionBadge(p.position)}`}>
                        {p.position}
                      </span>
                    </td>

                    {/* League */}
                    <td className="py-3 px-3 text-center">
                      <span className="text-[11px] text-slate-300 font-medium bg-slate-800/80 px-2 py-0.5 rounded-md border border-slate-700/40">
                        {p.competitionCode || p.competitionName}
                      </span>
                    </td>

                    {/* Overall Power Score */}
                    <td className="py-3 px-4 text-center">
                      <div className="inline-flex items-center space-x-1.5 px-2.5 py-1 rounded-xl bg-slate-900 border border-slate-700/60 shadow-inner">
                        <span className={`font-black text-sm bg-clip-text text-transparent bg-gradient-to-r ${getScoreColor(p.overallScore)}`}>
                          {p.overallScore.toFixed(1)}
                        </span>
                      </div>
                    </td>

                    {/* Goals / Assists */}
                    <td className="py-3 px-3 text-center">
                      <span className="font-semibold text-slate-200">{p.goals}</span>
                      <span className="text-slate-500 mx-1">/</span>
                      <span className="font-semibold text-emerald-400">{p.assists}</span>
                    </td>

                    {/* Subscores */}
                    <td className="py-3 px-3 text-center">
                      <span className="font-medium text-rose-400">{p.attackingScore.toFixed(0)}</span>
                    </td>
                    <td className="py-3 px-3 text-center">
                      <span className="font-medium text-amber-400">{p.playmakingScore.toFixed(0)}</span>
                    </td>
                    <td className="py-3 px-3 text-center">
                      <span className="font-medium text-blue-400">{p.defendingScore.toFixed(0)}</span>
                    </td>

                    {/* Market Value */}
                    <td className="py-3 px-4 text-right font-medium text-slate-300">
                      €{(p.marketValueEur / 1_000_000).toFixed(0)}M
                    </td>

                    {/* Action */}
                    <td className="py-3 px-3 text-center">
                      <button
                        onClick={(e) => {
                          e.stopPropagation();
                          onSelectPlayer(p);
                        }}
                        className="p-1.5 rounded-lg bg-slate-800 hover:bg-emerald-500 hover:text-slate-950 text-slate-400 transition-all"
                        title="View Detailed Radar"
                      >
                        <Eye className="w-3.5 h-3.5" />
                      </button>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};
