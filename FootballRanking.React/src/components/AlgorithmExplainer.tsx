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
            <span className="font-bold text-rose-400 block mb-1">Strikers (ST/CF)</span>
            <ul className="text-slate-400 space-y-1 text-[11px]">
              <li>• Attacking: <strong>75%</strong></li>
              <li>• Playmaking: <strong>15%</strong></li>
              <li>• Defending: <strong>2%</strong></li>
              <li>• Discipline: <strong>8%</strong></li>
            </ul>
          </div>

          <div className="bg-slate-900/80 p-3 rounded-xl border border-slate-800">
            <span className="font-bold text-amber-400 block mb-1">Wingers / CAMs</span>
            <ul className="text-slate-400 space-y-1 text-[11px]">
              <li>• Wingers (LW/RW): <strong>55% / 30% / 5% / 10%</strong></li>
              <li>• Attacking Mid (CAM): <strong>45% / 40% / 5% / 10%</strong></li>
              <li className="text-[10px] text-slate-500">(Att / Play / Def / Disc)</li>
            </ul>
          </div>

          <div className="bg-slate-900/80 p-3 rounded-xl border border-slate-800">
            <span className="font-bold text-blue-400 block mb-1">Midfield (CM / CDM)</span>
            <ul className="text-slate-400 space-y-1 text-[11px]">
              <li>• Central Mid (CM): <strong>25% / 45% / 20% / 10%</strong></li>
              <li>• Defensive Mid (CDM): <strong>15% / 35% / 40% / 10%</strong></li>
              <li className="text-[10px] text-slate-500">(Att / Play / Def / Disc)</li>
            </ul>
          </div>

          <div className="bg-slate-900/80 p-3 rounded-xl border border-slate-800">
            <span className="font-bold text-emerald-400 block mb-1">Defenders & Goalkeepers</span>
            <ul className="text-slate-400 space-y-1 text-[11px]">
              <li>• Center-Backs (CB): <strong>5% / 20% / 65% / 10%</strong></li>
              <li>• Fullbacks (LB/RB): <strong>20% / 35% / 35% / 10%</strong></li>
              <li>• Goalkeepers (GK): <strong>5% / 15% / 70% / 10%</strong></li>
            </ul>
          </div>
        </div>
      </div>

      {/* Pillar 2: Cross-League UEFA Coefficient Normalization */}
      <div className="bg-[#121826] border border-slate-800 rounded-2xl p-6 shadow-xl">
        <div className="flex items-center space-x-2 text-cyan-400 mb-3">
          <Scale className="w-5 h-5" />
          <h3 className="font-bold text-base text-white">2. Cross-League UEFA Normalization (LCE)</h3>
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
            <strong className="text-slate-200">Minutes Dampener (MNA):</strong> To prevent substitutes who play limited minutes from artificially dominating rankings, the engine applies a calibrated square-root dampening function:
          </p>
          <div className="p-3 bg-slate-900 rounded-xl font-mono text-emerald-300 text-[11px] leading-relaxed">
            <div>MNA = 1.0 (if Minutes ≥ Threshold)</div>
            <div>MNA = 0.20 (if Minutes ≤ 0)</div>
            <div>MNA = clamp(sqrt(Minutes / Threshold), 0.25, 1.0) (otherwise)</div>
          </div>
          <p>
            <strong className="text-slate-200">Disciplinary Sub-Score (S_disc):</strong> Yellow cards deduct 4 points, and red cards deduct 15 points from a baseline score of 100, clamped to [20, 100]:
          </p>
          <div className="p-3 bg-slate-900 rounded-xl font-mono text-emerald-300 text-[11px]">
            S_disc = clamp(100.0 - (YellowCards * 4.0 + RedCards * 15.0), 20.0, 100.0)
          </div>
          <p className="text-[11px] text-slate-400">
            This sub-score is factored into the composite rating via the positional discipline weight (8% for Strikers, 10% for all other positions), rewarding consistent fair play without causing negative score distortions.
          </p>
        </div>
      </div>
    </div>
  );
};
