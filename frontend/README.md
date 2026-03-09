# ASP.NET Core Learning Platform - Frontend

This is the frontend application for the ASP.NET Core Learning Platform, built with Next.js, TypeScript, and Tailwind CSS.

## Features

- **Next.js 15** with App Router for modern React development
- **TypeScript** for type safety
- **Tailwind CSS** with custom theme for styling
- **Axios** for API communication with the backend
- **Monaco Editor** integration for browser-based code editing (to be added)

## Getting Started

### Prerequisites

- Node.js 20.x or higher
- npm or yarn

### Installation

1. Install dependencies:

```bash
npm install
```

2. Create a `.env.local` file based on `.env.local.example`:

```bash
cp .env.local.example .env.local
```

3. Update the environment variables in `.env.local` to point to your backend API.

### Development

Run the development server:

```bash
npm run dev
```

Open [http://localhost:3000](http://localhost:3000) in your browser.

### Build

Build the application for production:

```bash
npm run build
```

### Start Production Server

```bash
npm start
```

## Project Structure

```
frontend/
├── app/                    # Next.js App Router pages
├── lib/                    # Utilities and API clients
│   ├── api/               # API service functions
│   │   ├── auth.ts        # Authentication API
│   │   ├── challenges.ts  # Challenges API
│   │   ├── courses.ts     # Courses API
│   │   ├── progress.ts    # Progress API
│   │   ├── code-execution.ts # Code execution API
│   │   └── ai-tutor.ts    # AI tutor API
│   ├── api-client.ts      # Axios instance with interceptors
│   └── types.ts           # TypeScript type definitions
├── public/                # Static assets
└── components/            # React components (to be added)
```

## API Client

The API client is configured with:

- Automatic authentication token injection
- Global error handling (401, 429, 503)
- 30-second timeout
- Request/response interceptors

## Custom Theme

The Tailwind theme includes custom colors for:

- Primary and secondary colors
- Success, warning, and error states
- Challenge difficulty levels (easy, medium, hard)
- Dark mode support

## Next Steps

- Add Monaco Editor integration for code editing
- Create authentication pages (login, register)
- Build dashboard component
- Implement challenge browser and detail pages
- Add course browser and lesson pages
- Create project pages
- Implement leaderboard page

