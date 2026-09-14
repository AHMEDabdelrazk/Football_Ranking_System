import React from 'react';
import { RankingWeights, PlayerRanking } from '../types/football';
import { Sliders, RefreshCw, Sparkles } from 'lucide-react';

interface AlgorithmSandboxProps {
  weights: RankingWeights;
  setWeights: React.Dispatch<React.SetStateAction<RankingWeights>>;
  players: PlayerRanking[];
  onReset: () => void;
}

export const AlgorithmSandbox: React.FC<AlgorithmSandboxProps> = ({
  weights,
  setWeights,
  players,
  onReset
}) => {
  const handleSliderChange = (key: keyof RankingWeights, value: number | boolean) => {
    setWeights(prev => ({
      ...prev,
      [key]: value
    }));
  };

  const applyPreset = (presetName: string) => {
    switch (presetName) {
      case 'poacher':
        setWeights({
          attackingWeight: 2.5,
          playmakingWeight: 0.5,
          defendingWeight: 0.1,
          disciplineWeight: 0.5,
          minutesThreshold: 360,
          applyLeagueCoefficient: true
        });
        break;
      case 'maestro':
        setWeights({
          attackingWeight: 0.8,
          playmakingWeight: 2.5,
          defendingWeight: 0.8,
          disciplineWeight: 1.0,
          minutesThreshold: 360,
          applyLeagueCoefficient: true
        });
        break;
      case 'colossus':
        setWeights({
          attackingWeight: 0.2,
          playmakingWeight: 0.8,
          defendingWeight: 2.8,
          disciplineWeight: 1.5,
          minutesThreshold: 360,
          applyLeagueCoefficient: true
        });
        break;
      case 'default':
        onReset();
        break;
    }
  };

  return (
    <div className="grid grid-cols-1 lg:grid-cols-12 gap-6">
      {/* Controls Column */}
      <div className="lg:col-span-5 space-y-4">
        <div className="bg-[#121826] border border-slate-800 rounded-2xl p-5 shadow-xl">
          <div className="flex items-center justify-between mb-4">
            <div className="flex items-center space-x-2 text-emerald-400">
              <Sliders className="w-5 h-5" />
              <h2 className="font-extrabold text-base text-white">Algorithm Hyperparameters</h2>
            </div>
            <button
              onClick={onReset}
              className="flex items-center space-x-1 text-xs text-slate-400 hover:text-emerald-400 transition-colors"
            >
              <RefreshCw className="w-3.5 h-3.5" />
              <span>Reset</span>
            </button>
          </div>

          <p className="text-xs text-slate-400 mb-5 leading-relaxed">
            Outlier Evaluator Mode: Adjust metric weights in real-time. The formula instantly recalculates composite role scores across all Top 5 European leagues.
          </p>

          {/* Presets */}
          <div className="mb-6">
            <span className="text-[11px] font-bold text-slate-400 uppercase tracking-wider block mb-2">
              Preset Evaluator Profiles
            </span>
            <div className="grid grid-cols-2 gap-2">
              <button
                onClick={() => applyPreset('default')}
                className="px-2.5 py-1.5 rounded-xl text-xs font-semibold bg-slate-800/80 hover:bg-slate-700 text-slate-300 transition-all text-left"
              >
                ⚖️ Balanced Fair Merit
              </button>
              <button
                onClick={() => applyPreset('poacher')}
                className="px-2.5 py-1.5 rounded-xl text-xs font-semibold bg-rose-500/10 hover:bg-rose-500/20 text-rose-400 border border-rose-500/20 transition-all text-left"
              >
                🎯 Goal Poachers (ST)
              </button>
              <button
                onClick={() => applyPreset('maestro')}
                className="px-2.5 py-1.5 rounded-xl text-xs font-semibold bg-amber-500/10 hover:bg-amber-500/20 text-amber-400 border border-amber-500/20 transition-all text-left"
              >
                🪄 Playmakers & Creators
              </button>
              <button
                onClick={() => applyPreset('colossus')}
                className="px-2.5 py-1.5 rounded-xl text-xs font-semibold bg-blue-500/10 hover:bg-blue-500/20 text-blue-400 border border-blue-500/20 transition-all text-left"
              >
                🛡️ Defensive Colossi
              </button>
            </div>
          </div>

          {/* Sliders */}
          <div className="space-y-4 text-xs">
            {/* Attacking Weight */}
            <div>
              <div className="flex justify-between font-semibold mb-1">
                <span className="text-rose-400">Attacking Weight (xG, Shots, Goals)</span>
                <span className="font-bold text-slate-200">{weights.attackingWeight.toFixed(1)}x</span>
              </div>
              <input
                type="range"
                min="0.1"
                max="3.0"
                step="0.1"
                value={weights.attackingWeight}
                onChange={(e) => handleSliderChange('attackingWeight', parseFloat(e.target.value))}
                className="w-full accent-rose-500 cursor-pointer"
              />
            </div>

            {/* Playmaking Weight */}
            <div>
              <div className="flex justify-between font-semibold mb-1">
                <span className="text-amber-400">Playmaking Weight (Assists, Key Passes, Carries)</span>
                <span className="font-bold text-slate-200">{weights.playmakingWeight.toFixed(1)}x</span>
              </div>
              <input
                type="range"
                min="0.1"
                max="3.0"
                step="0.1"
                value={weights.playmakingWeight}
                onChange={(e) => handleSliderChange('playmakingWeight', parseFloat(e.target.value))}
                className="w-full accent-amber-500 cursor-pointer"
              />
            </div>

            {/* Defending Weight */}
            <div>
              <div className="flex justify-between font-semibold mb-1">
                <span className="text-blue-400">Defending Weight (Tackles, Interceptions, Clean Sheets)</span>
                <span className="font-bold text-slate-200">{weights.defendingWeight.toFixed(1)}x</span>
              </div>
              <input
                type="range"
                min="0.1"
                max="3.0"
                step="0.1"
                value={weights.defendingWeight}
                onChange={(e) => handleSliderChange('defendingWeight', parseFloat(e.target.value))}
                className="w-full accent-blue-500 cursor-pointer"
              />
            </div>

            {/* Discipline Penalty */}
            <div>
              <div className="flex justify-between font-semibold mb-1">
                <span className="text-purple-400">Discipline Penalty (Yellow/Red Card Deductions)</span>
                <span className="font-bold text-slate-200">{weights.disciplineWeight.toFixed(1)}x</span>
              </div>
              <input
                type="range"
                min="0.1"
                max="3.0"
                step="0.1"
                value={weights.disciplineWeight}
                onChange={(e) => handleSliderChange('disciplineWeight', parseFloat(e.target.value))}
                className="w-full accent-purple-500 cursor-pointer"
              />
            </div>

            {/* Minutes Threshold */}
            <div>
              <div className="flex justify-between font-semibold mb-1">
                <span className="text-teal-400">Minutes Threshold (Anti-Sample Bias Dampener)</span>
                <span className="font-bold text-slate-200">{weights.minutesThreshold} mins</span>
              </div>
              <input
                type="range"
                min="90"
                max="900"
                step="30"
                value={weights.minutesThreshold}
                onChange={(e) => handleSliderChange('minutesThreshold', parseInt(e.target.value))}
                className="w-full accent-teal-500 cursor-pointer"
              />
            </div>

            {/* League Coefficient Switch */}
            <div className="pt-2 border-t border-slate-800 flex items-center justify-between">
              <div>
                <span className="font-bold text-slate-200 block">UEFA League Strength Coefficient</span>
                <span className="text-[10px] text-slate-500">Normalizes difficulty across Premier League, La Liga, etc.</span>
              </div>
              <input
                type="checkbox"
                checked={weights.applyLeagueCoefficient}
                onChange={(e) => handleSliderChange('applyLeagueCoefficient', e.target.checked)}
                className="w-4 h-4 accent-emerald-500 cursor-pointer"
              />
            </div>
          </div>
        </div>
      </div>

      {/* Live Re-Ranked Leaderboard Column */}
      <div className="lg:col-span-7 space-y-3">
        <div className="bg-[#121826] border border-slate-800 rounded-2xl p-4 shadow-xl">
          <div className="flex items-center justify-between mb-3">
            <div className="flex items-center space-x-2">
              <Sparkles className="w-4 h-4 text-emerald-400" />
              <h3 className="font-bold text-sm text-white">Live Re-Ranked Top 10</h3>
            </div>
            <span className="text-[11px] text-emerald-400 bg-emerald-500/10 px-2 py-0.5 rounded-full border border-emerald-500/20">
              Live Recalculated
            </span>
          </div>

          <div className="space-y-2">
            {players.slice(0, 10).map((player, idx) => (
              <div
                key={player.playerId}
                className="flex items-center justify-between p-2.5 rounded-xl bg-slate-900/60 hover:bg-slate-800/60 border border-slate-800/80 transition-all"
              >
                <div className="flex items-center space-x-3">
                  <span className={`w-6 text-center font-extrabold text-sm ${idx < 3 ? 'text-amber-400' : 'text-slate-400'}`}>
                    #{idx + 1}
                  </span>
                  <div className="w-8 h-8 rounded-full bg-slate-800 border border-slate-700 p-1 flex-shrink-0">
                    <img src={player.clubLogo} alt={player.clubName} className="w-full h-full object-contain" />
                  </div>
                  <div>
                    <div className="font-bold text-xs text-slate-100 flex items-center space-x-1.5">
                      <span>{player.name}</span>
                      <span className="text-[10px] px-1 bg-slate-800 rounded text-slate-400">{player.position}</span>
                    </div>
                    <div className="text-[10px] text-slate-400">
                      {player.clubName} • {player.competitionCode}
                    </div>
                  </div>
                </div>

                <div className="text-right">
                  <span className="text-base font-black text-emerald-400">
                    {player.overallScore.toFixed(1)}
                  </span>
                  <div className="text-[10px] text-slate-500">
                    Atk: {player.attackingScore.toFixed(0)} | Pl: {player.playmakingScore.toFixed(0)} | Def: {player.defendingScore.toFixed(0)}
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};
