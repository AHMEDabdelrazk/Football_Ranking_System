import React from 'react';
import { Calculator, ShieldCheck, Scale, Cpu } from 'lucide-react';

export const AlgorithmExplainer: React.FC = () => {
  return (
    <div className="max-w-4xl mx-auto space-y-6">
      {/* Header */}
      <div className="bg-[#121826] border border-slate-800 rounded-2xl p-6 shadow-xl">
        <div className="flex items-center space-x-3 mb-2">
          <div className="p-2 rounded-xl bg-purple-500/10 text-purple-400 border border-purple-500/30">
            <Calculator className="w-6 h-6" />
          </div>
          <h2 className="text-xl font-black text-white">Algorithmic Ranking Methodology</h2>
        </div>
        <p className="text-xs text-slate-400 leading-relaxed">
          Traditional football rankings naively tally raw goals or subjective media votes, resulting in severe positional bias and league inflation. Our platform implements a multi-dimensional, role-specific, league-normalized mathematical scoring engine.
        </p>
      </div>

      {/* Pillar 1: Role-Specific Evaluation Matrices */}
      <div className="bg-[#121826] border border-slate-800 rounded-2xl p-6 shadow-xl">
        <div className="flex items-center space-x-2 text-emerald-400 mb-3">
          <Cpu className="w-5 h-5" />
          <h3 className="font-bold text-base text-white">1. Dynamic Positional Weight Matrices (W_pos)</h3>
        </div>
        <p className="text-xs text-slate-400 mb-4 leading-relaxed">
          A center-back cannot be judged on goals scored, nor a striker on tackles won. The engine applies dynamically tailored evaluation vectors:
        </p>

        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-3 text-xs">
          <div className="bg-slate-900/80 p-3 rounded-xl border border-slate-800">
            <span className="font-bold text-rose-400 block mb-1">Strikers (ST)</span>
            <ul className="text-slate-400 space-y-1 text-[11px]">
              <li>• Attacking: <strong>75%</strong></li>
              <li>• Playmaking: <strong>15%</strong></li>
              <li>• Discipline: <strong>8%</strong></li>
              <li>• Defending: <strong>2%</strong></li>
            </ul>
          </div>

          <div className="bg-slate-900/80 p-3 rounded-xl border border-slate-800">
            <span className="font-bold text-amber-400 block mb-1">Wingers / CAMs</span>
            <ul className="text-slate-400 space-y-1 text-[11px]">
              <li>• Attacking: <strong>50%</strong></li>
              <li>• Playmaking: <strong>35%</strong></li>
              <li>• Defending: <strong>5%</strong></li>
              <li>• Discipline: <strong>10%</strong></li>
            </ul>
          </div>

          <div className="bg-slate-900/80 p-3 rounded-xl border border-slate-800">
            <span className="font-bold text-blue-400 block mb-1">Midfield / CDM</span>
            <ul className="text-slate-400 space-y-1 text-[11px]">
              <li>• Playmaking: <strong>45%</strong></li>
              <li>• Defending: <strong>35%</strong></li>
              <li>• Attacking: <strong>10%</strong></li>
              <li>• Discipline: <strong>10%</strong></li>
            </ul>
          </div>

          <div className="bg-slate-900/80 p-3 rounded-xl border border-slate-800">
            <span className="font-bold text-emerald-400 block mb-1">Defenders / GK</span>
            <ul className="text-slate-400 space-y-1 text-[11px]">
              <li>• Defending: <strong>70%</strong></li>
              <li>• Playmaking: <strong>15%</strong></li>
              <li>• Attacking: <strong>5%</strong></li>
              <li>• Discipline: <strong>10%</strong></li>
            </ul>
          </div>
        </div>
      </div>

      {/* Pillar 2: Cross-League UEFA Coefficient Normalization */}
      <div className="bg-[#121826] border border-slate-800 rounded-2xl p-6 shadow-xl">
        <div className="flex items-center space-x-2 text-cyan-400 mb-3">
          <Scale className="w-5 h-5" />
          <h3 className="font-bold text-base text-white">2. Cross-League UEFA Normalization ($LCE$)</h3>
        </div>
        <p className="text-xs text-slate-400 mb-3 leading-relaxed">
          Scoring 20 goals in one league is not mathematically identical to scoring 20 goals in another due to defensive density and league competitiveness. The engine multiplies raw performance metrics by official UEFA league coefficient weights:
        </p>

        <div className="grid grid-cols-2 sm:grid-cols-5 gap-2 text-center text-xs">
          <div className="bg-slate-900 p-2.5 rounded-xl border border-slate-800">
            <span className="text-slate-400 block text-[10px]">Premier League</span>
            <span className="font-black text-emerald-400 text-sm">1.00x</span>
          </div>
          <div className="bg-slate-900 p-2.5 rounded-xl border border-slate-800">
            <span className="text-slate-400 block text-[10px]">La Liga</span>
            <span className="font-black text-emerald-400 text-sm">0.98x</span>
          </div>
          <div className="bg-slate-900 p-2.5 rounded-xl border border-slate-800">
            <span className="text-slate-400 block text-[10px]">Serie A</span>
            <span className="font-black text-emerald-400 text-sm">0.96x</span>
          </div>
          <div className="bg-slate-900 p-2.5 rounded-xl border border-slate-800">
            <span className="text-slate-400 block text-[10px]">Bundesliga</span>
            <span className="font-black text-emerald-400 text-sm">0.95x</span>
          </div>
          <div className="bg-slate-900 p-2.5 rounded-xl border border-slate-800">
            <span className="text-slate-400 block text-[10px]">Ligue 1</span>
            <span className="font-black text-emerald-400 text-sm">0.92x</span>
          </div>
        </div>
      </div>

      {/* Pillar 3: Minutes Dampener & Disciplinary Deductions */}
      <div className="bg-[#121826] border border-slate-800 rounded-2xl p-6 shadow-xl">
        <div className="flex items-center space-x-2 text-amber-400 mb-3">
          <ShieldCheck className="w-5 h-5" />
          <h3 className="font-bold text-base text-white">3. Minutes Dampening & Disciplinary Governance</h3>
        </div>
        <div className="space-y-3 text-xs text-slate-400 leading-relaxed">
          <p>
            <strong className="text-slate-200">Minutes Dampener ($MNA$):</strong> To prevent substitutes who score a single goal in 15 minutes from unnaturally topping the leaderboard, the engine applies a smooth square-root threshold function:
          </p>
          <div className="p-3 bg-slate-900 rounded-xl font-mono text-emerald-300 text-[11px]">
            Dampener = min(1.0, sqrt(MinutesPlayed / Threshold))
          </div>
          <p>
            <strong className="text-slate-200">Disciplinary Adjustment ($DP$):</strong> Yellow cards deduct 4 points, and red cards deduct 15 points from the discipline vector, reflecting reckless conduct that harms team probability.
          </p>
        </div>
      </div>
    </div>
  );
};
