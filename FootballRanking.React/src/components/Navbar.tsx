import React from 'react';
import { Trophy, Sliders, Shield, BookOpen, Layers } from 'lucide-react';
import { Competition } from '../types/football';

interface NavbarProps {
  activeTab: 'players' | 'clubs' | 'sandbox' | 'explainer';
  setActiveTab: (tab: 'players' | 'clubs' | 'sandbox' | 'explainer') => void;
  competitions: Competition[];
  selectedLeague: string;
  setSelectedLeague: (code: string) => void;
}

export const Navbar: React.FC<NavbarProps> = ({
  activeTab,
  setActiveTab,
  competitions,
  selectedLeague,
  setSelectedLeague
}) => {
  return (
    <header className="sticky top-0 z-40 bg-[#0c1220]/90 backdrop-blur-md border-b border-slate-800/80">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-16">
          {/* Logo & Title */}
          <div className="flex items-center space-x-3 cursor-pointer" onClick={() => setActiveTab('players')}>
            <div className="w-10 h-10 rounded-xl bg-gradient-to-tr from-emerald-500 to-cyan-500 flex items-center justify-center shadow-lg shadow-emerald-500/20">
              <Trophy className="w-5 h-5 text-white" />
            </div>
            <div>
              <span className="text-xl font-extrabold tracking-tight bg-clip-text text-transparent bg-gradient-to-r from-white via-slate-200 to-emerald-400">
                FOOTBALL<span className="text-emerald-400">RANK</span>
              </span>
              <span className="hidden sm:inline-block ml-2 px-2 py-0.5 text-[10px] font-bold tracking-wider uppercase rounded-full bg-emerald-500/10 text-emerald-400 border border-emerald-500/30">
                Top 5 Leagues Engine
              </span>
            </div>
          </div>

          {/* Navigation Tabs */}
          <nav className="flex items-center space-x-1 sm:space-x-2">
            <button
              onClick={() => setActiveTab('players')}
              className={`flex items-center space-x-2 px-3 py-1.5 rounded-lg text-sm font-semibold transition-all ${
                activeTab === 'players'
                  ? 'bg-emerald-500/15 text-emerald-400 border border-emerald-500/30 shadow-sm'
                  : 'text-slate-400 hover:text-slate-200 hover:bg-slate-800/50'
              }`}
            >
              <Trophy className="w-4 h-4" />
              <span>Players</span>
            </button>

            <button
              onClick={() => setActiveTab('clubs')}
              className={`flex items-center space-x-2 px-3 py-1.5 rounded-lg text-sm font-semibold transition-all ${
                activeTab === 'clubs'
                  ? 'bg-emerald-500/15 text-emerald-400 border border-emerald-500/30 shadow-sm'
                  : 'text-slate-400 hover:text-slate-200 hover:bg-slate-800/50'
              }`}
            >
              <Shield className="w-4 h-4" />
              <span>Clubs</span>
            </button>

            <button
              onClick={() => setActiveTab('sandbox')}
              className={`flex items-center space-x-2 px-3 py-1.5 rounded-lg text-sm font-semibold transition-all ${
                activeTab === 'sandbox'
                  ? 'bg-cyan-500/15 text-cyan-400 border border-cyan-500/30 shadow-sm'
                  : 'text-slate-400 hover:text-slate-200 hover:bg-slate-800/50'
              }`}
            >
              <Sliders className="w-4 h-4" />
              <span>Sandbox</span>
            </button>

            <button
              onClick={() => setActiveTab('explainer')}
              className={`flex items-center space-x-2 px-3 py-1.5 rounded-lg text-sm font-semibold transition-all ${
                activeTab === 'explainer'
                  ? 'bg-purple-500/15 text-purple-400 border border-purple-500/30 shadow-sm'
                  : 'text-slate-400 hover:text-slate-200 hover:bg-slate-800/50'
              }`}
            >
              <BookOpen className="w-4 h-4" />
              <span className="hidden md:inline">Methodology</span>
            </button>
          </nav>
        </div>

        {/* League Selector Ribbon */}
        <div className="flex items-center space-x-2 py-2 overflow-x-auto no-scrollbar border-t border-slate-800/50">
          <button
            onClick={() => setSelectedLeague('')}
            className={`flex items-center space-x-1.5 px-3 py-1 rounded-full text-xs font-semibold whitespace-nowrap transition-all ${
              selectedLeague === ''
                ? 'bg-gradient-to-r from-emerald-600 to-teal-600 text-white shadow-sm shadow-emerald-500/25'
                : 'bg-slate-800/60 text-slate-400 hover:text-slate-200 hover:bg-slate-800'
            }`}
          >
            <Layers className="w-3.5 h-3.5" />
            <span>All Top 5 Leagues</span>
          </button>

          {competitions.map((comp) => (
            <button
              key={comp.competitionId}
              onClick={() => setSelectedLeague(comp.code || '')}
              className={`flex items-center space-x-1.5 px-3 py-1 rounded-full text-xs font-semibold whitespace-nowrap transition-all ${
                selectedLeague === comp.code
                  ? 'bg-gradient-to-r from-emerald-600 to-teal-600 text-white shadow-sm shadow-emerald-500/25'
                  : 'bg-slate-800/60 text-slate-400 hover:text-slate-200 hover:bg-slate-800'
              }`}
            >
              {comp.logo && <img src={comp.logo} alt={comp.name} className="w-3.5 h-3.5 object-contain" />}
              <span>{comp.name}</span>
              <span className="text-[10px] text-slate-400">({comp.coefficientWeight}x)</span>
            </button>
          ))}
        </div>
      </div>
    </header>
  );
};
