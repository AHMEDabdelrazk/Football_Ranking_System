import React, { useState, useEffect, useCallback } from 'react';
import { Navbar } from './components/Navbar';
import { PlayerLeaderboard } from './components/PlayerLeaderboard';
import { ClubPowerRankings } from './components/ClubPowerRankings';
import { AlgorithmSandbox } from './components/AlgorithmSandbox';
import { AlgorithmExplainer } from './components/AlgorithmExplainer';
import { PlayerModal } from './components/PlayerModal';
import { PlayerRanking, ClubRanking, Competition, RankingWeights } from './types/football';
import { fetchPlayers, fetchClubs, fetchCompetitions, DEFAULT_WEIGHTS } from './services/api';

export const App: React.FC = () => {
  const [activeTab, setActiveTab] = useState<'players' | 'clubs' | 'sandbox' | 'explainer'>('players');
  const [competitions, setCompetitions] = useState<Competition[]>([]);
  const [selectedLeague, setSelectedLeague] = useState<string>('');
  const [positionFilter, setPositionFilter] = useState<string>('');
  const [searchQuery, setSearchQuery] = useState<string>('');
  const [sortBy, setSortBy] = useState<string>('overallScore');
  const [sortDescending, setSortDescending] = useState<boolean>(true);

  const [weights, setWeights] = useState<RankingWeights>(DEFAULT_WEIGHTS);
  const [players, setPlayers] = useState<PlayerRanking[]>([]);
  const [clubs, setClubs] = useState<ClubRanking[]>([]);
  const [selectedPlayer, setSelectedPlayer] = useState<PlayerRanking | null>(null);
  const [loading, setLoading] = useState<boolean>(true);

  // Load competitions on mount
  useEffect(() => {
    fetchCompetitions().then(setCompetitions).catch(console.error);
  }, []);

  // Load players based on filters and weights
  const loadPlayers = useCallback(async () => {
    setLoading(true);
    try {
      const res = await fetchPlayers(
        {
          competitionCode: selectedLeague || undefined,
          position: positionFilter || undefined,
          searchQuery: searchQuery || undefined,
          sortBy,
          sortDescending
        },
        weights
      );
      setPlayers(res.items);
    } catch (err) {
      console.error("Failed to load players", err);
    } finally {
      setLoading(false);
    }
  }, [selectedLeague, positionFilter, searchQuery, sortBy, sortDescending, weights]);

  // Load clubs
  const loadClubs = useCallback(async () => {
    setLoading(true);
    try {
      const res = await fetchClubs(selectedLeague || undefined);
      setClubs(res);
    } catch (err) {
      console.error("Failed to load clubs", err);
    } finally {
      setLoading(false);
    }
  }, [selectedLeague]);

  useEffect(() => {
    if (activeTab === 'players' || activeTab === 'sandbox') {
      loadPlayers();
    } else if (activeTab === 'clubs') {
      loadClubs();
    }
  }, [activeTab, loadPlayers, loadClubs]);

  const handleResetWeights = () => {
    setWeights(DEFAULT_WEIGHTS);
  };

  return (
    <div className="min-h-screen bg-[#0a0e17] text-slate-100 flex flex-col selection:bg-emerald-500 selection:text-slate-950">
      {/* Navbar */}
      <Navbar
        activeTab={activeTab}
        setActiveTab={setActiveTab}
        competitions={competitions}
        selectedLeague={selectedLeague}
        setSelectedLeague={setSelectedLeague}
      />

      {/* Main Content Body */}
      <main className="flex-1 max-w-7xl w-full mx-auto px-4 sm:px-6 lg:px-8 py-6">
        {activeTab === 'players' && (
          <PlayerLeaderboard
            players={players}
            loading={loading}
            onSelectPlayer={setSelectedPlayer}
            searchQuery={searchQuery}
            setSearchQuery={setSearchQuery}
            positionFilter={positionFilter}
            setPositionFilter={setPositionFilter}
            sortBy={sortBy}
            setSortBy={setSortBy}
            sortDescending={sortDescending}
            setSortDescending={setSortDescending}
          />
        )}

        {activeTab === 'clubs' && (
          <ClubPowerRankings
            clubs={clubs}
            loading={loading}
          />
        )}

        {activeTab === 'sandbox' && (
          <AlgorithmSandbox
            weights={weights}
            setWeights={setWeights}
            players={players}
            onReset={handleResetWeights}
          />
        )}

        {activeTab === 'explainer' && (
          <AlgorithmExplainer />
        )}
      </main>

      {/* Deep-Dive Player Modal */}
      <PlayerModal
        player={selectedPlayer}
        onClose={() => setSelectedPlayer(null)}
      />

      {/* Footer */}
      <footer className="border-t border-slate-800/60 py-6 bg-[#080c14] text-center text-xs text-slate-500">
        <div className="max-w-7xl mx-auto px-4 flex flex-col sm:flex-row items-center justify-between gap-2">
          <span>Enterprise Football Ranking Platform • Top 5 European Leagues Engine</span>
          <span className="text-[11px] text-slate-600">
            Powered by .NET 8 Web API & React 18 TypeScript Architecture
          </span>
        </div>
      </footer>
    </div>
  );
};
export default App;
