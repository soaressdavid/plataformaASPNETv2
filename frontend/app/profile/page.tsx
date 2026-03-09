'use client';

import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { ProtectedRoute } from '@/lib/components/ProtectedRoute';
import { Navigation } from '@/lib/components/Navigation';
import { Icons } from '@/lib/components/Icons';
import apiClient from '@/lib/api-client';
import toast from 'react-hot-toast';

interface UserProfile {
  id: number;
  name: string;
  email: string;
  level: number;
  totalXP: number;
  currentStreak: number;
  longestStreak: number;
  joinedAt: string;
  completedChallenges: number;
  completedLessons: number;
  totalProjects: number;
  badges: number;
  rank: number;
  bio?: string;
  location?: string;
  website?: string;
  github?: string;
  linkedin?: string;
}

interface ActivityItem {
  id: number;
  type: 'challenge' | 'lesson' | 'project' | 'achievement';
  title: string;
  description: string;
  xpGained: number;
  timestamp: string;
}

/**
 * User Profile Page
 * Task 19.1: Display user stats, activity history, profile customization
 * Requirements: 26.1, 26.2
 */
export default function ProfilePage() {
  const router = useRouter();
  const [profile, setProfile] = useState<UserProfile | null>(null);
  const [activities, setActivities] = useState<ActivityItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [isEditing, setIsEditing] = useState(false);
  const [editForm, setEditForm] = useState({
    bio: '',
    location: '',
    website: '',
    github: '',
    linkedin: '',
  });

  useEffect(() => {
    fetchProfile();
    fetchActivities();
  }, []);

  const fetchProfile = async () => {
    try {
      const response = await apiClient.get('/api/users/profile');
      setProfile(response.data);
      setEditForm({
        bio: response.data.bio || '',
        location: response.data.location || '',
        website: response.data.website || '',
        github: response.data.github || '',
        linkedin: response.data.linkedin || '',
      });
    } catch (error) {
      console.error('Failed to fetch profile:', error);
      toast.error('Failed to load profile');
    } finally {
      setLoading(false);
    }
  };

  const fetchActivities = async () => {
    try {
      const response = await apiClient.get('/api/users/activity');
      setActivities(response.data);
    } catch (error) {
      console.error('Failed to fetch activities:', error);
    }
  };

  const handleSaveProfile = async () => {
    try {
      await apiClient.put('/api/users/profile', editForm);
      toast.success('Profile updated successfully');
      setIsEditing(false);
      fetchProfile();
    } catch (error) {
      console.error('Failed to update profile:', error);
      toast.error('Failed to update profile');
    }
  };

  const getXPToNextLevel = (level: number, currentXP: number) => {
    const nextLevelXP = Math.pow(level + 1, 2) * 100;
    return nextLevelXP - currentXP;
  };

  const getActivityIcon = (type: string) => {
    switch (type) {
      case 'challenge':
        return <Icons.Code className="w-5 h-5 text-blue-600" />;
      case 'lesson':
        return <Icons.Book className="w-5 h-5 text-green-600" />;
      case 'project':
        return <Icons.Folder className="w-5 h-5 text-purple-600" />;
      case 'achievement':
        return <Icons.Trophy className="w-5 h-5 text-yellow-600" />;
      default:
        return <Icons.Star className="w-5 h-5 text-gray-600" />;
    }
  };

  if (loading) {
    return (
      <ProtectedRoute>
        <div className="min-h-screen bg-gray-50">
          <Navigation />
          <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
            <div className="animate-pulse">
              <div className="h-48 bg-gray-200 rounded-lg mb-8" />
              <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
                <div className="lg:col-span-2 space-y-4">
                  <div className="h-64 bg-gray-200 rounded-lg" />
                  <div className="h-96 bg-gray-200 rounded-lg" />
                </div>
                <div className="space-y-4">
                  <div className="h-64 bg-gray-200 rounded-lg" />
                </div>
              </div>
            </div>
          </div>
        </div>
      </ProtectedRoute>
    );
  }

  if (!profile) {
    return (
      <ProtectedRoute>
        <div className="min-h-screen bg-gray-50">
          <Navigation />
          <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
            <div className="text-center">
              <p className="text-gray-600">Profile not found</p>
            </div>
          </div>
        </div>
      </ProtectedRoute>
    );
  }

  const xpToNextLevel = getXPToNextLevel(profile.level, profile.totalXP);
  const currentLevelXP = Math.pow(profile.level, 2) * 100;
  const nextLevelXP = Math.pow(profile.level + 1, 2) * 100;
  const levelProgress = ((profile.totalXP - currentLevelXP) / (nextLevelXP - currentLevelXP)) * 100;

  return (
    <ProtectedRoute>
      <div className="min-h-screen bg-gray-50">
        <Navigation />

        <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          {/* Profile Header */}
          <div className="bg-linear-to-r from-primary-600 to-primary-700 rounded-lg shadow-lg p-8 mb-8 text-white">
            <div className="flex items-start justify-between">
              <div className="flex items-center gap-6">
                {/* Avatar */}
                <div className="w-24 h-24 bg-white rounded-full flex items-center justify-center text-4xl font-bold text-primary-600">
                  {profile.name.charAt(0).toUpperCase()}
                </div>

                {/* User Info */}
                <div>
                  <h1 className="text-3xl font-bold mb-2">{profile.name}</h1>
                  <p className="text-primary-100 mb-3">{profile.email}</p>
                  <div className="flex items-center gap-4 text-sm">
                    <div className="flex items-center gap-1">
                      <Icons.Calendar className="w-4 h-4" />
                      <span>Joined {new Date(profile.joinedAt).toLocaleDateString()}</span>
                    </div>
                    <div className="flex items-center gap-1">
                      <Icons.Trophy className="w-4 h-4" />
                      <span>Rank #{profile.rank}</span>
                    </div>
                  </div>
                </div>
              </div>

              {/* Edit Button */}
              <button
                onClick={() => setIsEditing(!isEditing)}
                className="px-4 py-2 bg-white text-primary-600 rounded-lg hover:bg-primary-50 transition-colors font-medium"
              >
                {isEditing ? 'Cancel' : 'Edit Profile'}
              </button>
            </div>

            {/* Level Progress */}
            <div className="mt-6">
              <div className="flex items-center justify-between mb-2">
                <div className="flex items-center gap-3">
                  <span className="text-2xl font-bold">Level {profile.level}</span>
                  <span className="text-primary-100">
                    {profile.totalXP.toLocaleString()} XP
                  </span>
                </div>
                <span className="text-primary-100">
                  {xpToNextLevel.toLocaleString()} XP to Level {profile.level + 1}
                </span>
              </div>
              <div className="w-full bg-primary-500 rounded-full h-3">
                <div
                  className="bg-white h-3 rounded-full transition-all duration-500"
                  style={{ width: `${levelProgress}%` }}
                />
              </div>
            </div>
          </div>

          <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
            {/* Main Content */}
            <div className="lg:col-span-2 space-y-6">
              {/* Edit Form */}
              {isEditing && (
                <div className="bg-white rounded-lg shadow-sm p-6">
                  <h2 className="text-xl font-semibold text-gray-900 mb-4">
                    Edit Profile
                  </h2>
                  <div className="space-y-4">
                    <div>
                      <label className="block text-sm font-medium text-gray-700 mb-1">
                        Bio
                      </label>
                      <textarea
                        value={editForm.bio}
                        onChange={(e) => setEditForm({ ...editForm, bio: e.target.value })}
                        rows={3}
                        className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                        placeholder="Tell us about yourself..."
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700 mb-1">
                        Location
                      </label>
                      <input
                        type="text"
                        value={editForm.location}
                        onChange={(e) => setEditForm({ ...editForm, location: e.target.value })}
                        className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                        placeholder="City, Country"
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700 mb-1">
                        Website
                      </label>
                      <input
                        type="url"
                        value={editForm.website}
                        onChange={(e) => setEditForm({ ...editForm, website: e.target.value })}
                        className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                        placeholder="https://yourwebsite.com"
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700 mb-1">
                        GitHub
                      </label>
                      <input
                        type="text"
                        value={editForm.github}
                        onChange={(e) => setEditForm({ ...editForm, github: e.target.value })}
                        className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                        placeholder="username"
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700 mb-1">
                        LinkedIn
                      </label>
                      <input
                        type="text"
                        value={editForm.linkedin}
                        onChange={(e) => setEditForm({ ...editForm, linkedin: e.target.value })}
                        className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                        placeholder="username"
                      />
                    </div>
                    <button
                      onClick={handleSaveProfile}
                      className="w-full px-4 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700 transition-colors font-medium"
                    >
                      Save Changes
                    </button>
                  </div>
                </div>
              )}

              {/* About */}
              {!isEditing && (
                <div className="bg-white rounded-lg shadow-sm p-6">
                  <h2 className="text-xl font-semibold text-gray-900 mb-4">About</h2>
                  {profile.bio ? (
                    <p className="text-gray-700 mb-4">{profile.bio}</p>
                  ) : (
                    <p className="text-gray-500 italic mb-4">No bio yet</p>
                  )}
                  <div className="space-y-2">
                    {profile.location && (
                      <div className="flex items-center gap-2 text-gray-600">
                        <Icons.MapPin className="w-4 h-4" />
                        <span>{profile.location}</span>
                      </div>
                    )}
                    {profile.website && (
                      <div className="flex items-center gap-2 text-gray-600">
                        <Icons.Link className="w-4 h-4" />
                        <a
                          href={profile.website}
                          target="_blank"
                          rel="noopener noreferrer"
                          className="text-primary-600 hover:underline"
                        >
                          {profile.website}
                        </a>
                      </div>
                    )}
                    {profile.github && (
                      <div className="flex items-center gap-2 text-gray-600">
                        <Icons.Github className="w-4 h-4" />
                        <a
                          href={`https://github.com/${profile.github}`}
                          target="_blank"
                          rel="noopener noreferrer"
                          className="text-primary-600 hover:underline"
                        >
                          @{profile.github}
                        </a>
                      </div>
                    )}
                    {profile.linkedin && (
                      <div className="flex items-center gap-2 text-gray-600">
                        <Icons.Linkedin className="w-4 h-4" />
                        <a
                          href={`https://linkedin.com/in/${profile.linkedin}`}
                          target="_blank"
                          rel="noopener noreferrer"
                          className="text-primary-600 hover:underline"
                        >
                          {profile.linkedin}
                        </a>
                      </div>
                    )}
                  </div>
                </div>
              )}

              {/* Activity History */}
              <div className="bg-white rounded-lg shadow-sm p-6">
                <h2 className="text-xl font-semibold text-gray-900 mb-4">
                  Recent Activity
                </h2>
                {activities.length > 0 ? (
                  <div className="space-y-4">
                    {activities.map((activity) => (
                      <div
                        key={activity.id}
                        className="flex items-start gap-4 p-4 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors"
                      >
                        <div className="mt-1">{getActivityIcon(activity.type)}</div>
                        <div className="flex-1">
                          <h3 className="font-medium text-gray-900">{activity.title}</h3>
                          <p className="text-sm text-gray-600 mt-1">{activity.description}</p>
                          <div className="flex items-center gap-4 mt-2 text-xs text-gray-500">
                            <span>{new Date(activity.timestamp).toLocaleDateString()}</span>
                            <span className="flex items-center gap-1">
                              <Icons.Star className="w-3 h-3 text-yellow-500" />
                              +{activity.xpGained} XP
                            </span>
                          </div>
                        </div>
                      </div>
                    ))}
                  </div>
                ) : (
                  <p className="text-gray-500 text-center py-8">No recent activity</p>
                )}
              </div>
            </div>

            {/* Sidebar */}
            <div className="space-y-6">
              {/* Stats Card */}
              <div className="bg-white rounded-lg shadow-sm p-6">
                <h2 className="text-xl font-semibold text-gray-900 mb-4">Statistics</h2>
                <div className="space-y-4">
                  <div className="flex items-center justify-between">
                    <span className="text-gray-600">Challenges Completed</span>
                    <span className="text-2xl font-bold text-primary-600">
                      {profile.completedChallenges}
                    </span>
                  </div>
                  <div className="flex items-center justify-between">
                    <span className="text-gray-600">Lessons Completed</span>
                    <span className="text-2xl font-bold text-green-600">
                      {profile.completedLessons}
                    </span>
                  </div>
                  <div className="flex items-center justify-between">
                    <span className="text-gray-600">Projects</span>
                    <span className="text-2xl font-bold text-purple-600">
                      {profile.totalProjects}
                    </span>
                  </div>
                  <div className="flex items-center justify-between">
                    <span className="text-gray-600">Badges Earned</span>
                    <span className="text-2xl font-bold text-yellow-600">
                      {profile.badges}
                    </span>
                  </div>
                </div>
              </div>

              {/* Streak Card */}
              <div className="bg-linear-to-br from-orange-500 to-red-500 rounded-lg shadow-lg p-6 text-white">
                <h2 className="text-xl font-semibold mb-4">Streak</h2>
                <div className="text-center">
                  <div className="text-5xl font-bold mb-2">🔥</div>
                  <div className="text-4xl font-bold mb-1">{profile.currentStreak}</div>
                  <div className="text-orange-100 mb-4">Day Streak</div>
                  <div className="text-sm text-orange-100">
                    Longest: {profile.longestStreak} days
                  </div>
                </div>
              </div>

              {/* Quick Actions */}
              <div className="bg-white rounded-lg shadow-sm p-6">
                <h2 className="text-xl font-semibold text-gray-900 mb-4">Quick Actions</h2>
                <div className="space-y-2">
                  <button
                    onClick={() => router.push('/portfolio')}
                    className="w-full px-4 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700 transition-colors text-left flex items-center gap-2"
                  >
                    <Icons.Folder className="w-4 h-4" />
                    View Portfolio
                  </button>
                  <button
                    onClick={() => router.push('/achievements')}
                    className="w-full px-4 py-2 bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200 transition-colors text-left flex items-center gap-2"
                  >
                    <Icons.Trophy className="w-4 h-4" />
                    View Achievements
                  </button>
                  <button
                    onClick={() => router.push('/missions')}
                    className="w-full px-4 py-2 bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200 transition-colors text-left flex items-center gap-2"
                  >
                    <Icons.Target className="w-4 h-4" />
                    View Missions
                  </button>
                </div>
              </div>
            </div>
          </div>
        </main>
      </div>
    </ProtectedRoute>
  );
}
