import { NextResponse } from 'next/server';

export async function GET() {
  try {
    const apiUrl = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';
    console.log('Testing API connection to:', apiUrl);
    
    const response = await fetch(`${apiUrl}/api/courses`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    });
    
    const data = await response.json();
    
    return NextResponse.json({
      success: true,
      apiUrl,
      status: response.status,
      data
    });
  } catch (error: any) {
    console.error('API test error:', error);
    return NextResponse.json({
      success: false,
      error: error.message
    }, { status: 500 });
  }
}
