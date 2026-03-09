import { render, screen } from '@testing-library/react';
import { Breadcrumb, BreadcrumbItem } from '../Breadcrumb';

describe('Breadcrumb', () => {
  it('renders breadcrumb items with separators', () => {
    const items: BreadcrumbItem[] = [
      { label: 'Home', href: '/' },
      { label: 'Courses', href: '/courses' },
      { label: 'Current Course' },
    ];

    render(<Breadcrumb items={items} />);

    expect(screen.getByText('Home')).toBeInTheDocument();
    expect(screen.getByText('Courses')).toBeInTheDocument();
    expect(screen.getByText('Current Course')).toBeInTheDocument();
  });

  it('renders links for items with href', () => {
    const items: BreadcrumbItem[] = [
      { label: 'Home', href: '/' },
      { label: 'Courses', href: '/courses' },
      { label: 'Current Course' },
    ];

    render(<Breadcrumb items={items} />);

    const homeLink = screen.getByRole('link', { name: 'Home' });
    const coursesLink = screen.getByRole('link', { name: 'Courses' });

    expect(homeLink).toHaveAttribute('href', '/');
    expect(coursesLink).toHaveAttribute('href', '/courses');
  });

  it('renders last item as non-clickable text', () => {
    const items: BreadcrumbItem[] = [
      { label: 'Home', href: '/' },
      { label: 'Current Page' },
    ];

    render(<Breadcrumb items={items} />);

    const lastItem = screen.getByText('Current Page');
    expect(lastItem.tagName).toBe('SPAN');
    expect(lastItem).not.toHaveAttribute('href');
  });

  it('has accessible aria-label', () => {
    const items: BreadcrumbItem[] = [
      { label: 'Home', href: '/' },
      { label: 'Current' },
    ];

    render(<Breadcrumb items={items} />);

    const nav = screen.getByRole('navigation');
    expect(nav).toHaveAttribute('aria-label', 'Breadcrumb');
  });

  it('renders single item without separator', () => {
    const items: BreadcrumbItem[] = [{ label: 'Only Item' }];

    render(<Breadcrumb items={items} />);

    expect(screen.getByText('Only Item')).toBeInTheDocument();
    // No separator should be rendered for single item
    const nav = screen.getByRole('navigation');
    expect(nav.querySelectorAll('svg').length).toBe(0);
  });

  it('applies correct styling to last item', () => {
    const items: BreadcrumbItem[] = [
      { label: 'Home', href: '/' },
      { label: 'Current Page' },
    ];

    render(<Breadcrumb items={items} />);

    const lastItem = screen.getByText('Current Page');
    expect(lastItem).toHaveClass('text-gray-900', 'font-medium');
  });

  it('renders multiple items with correct number of separators', () => {
    const items: BreadcrumbItem[] = [
      { label: 'Level 1', href: '/levels/1' },
      { label: 'Course 1', href: '/courses/1' },
      { label: 'Lesson 1', href: '/lessons/1' },
      { label: 'Current Section' },
    ];

    render(<Breadcrumb items={items} />);

    const nav = screen.getByRole('navigation');
    // Should have 3 separators for 4 items
    expect(nav.querySelectorAll('svg').length).toBe(3);
  });

  it('handles items without href as non-clickable', () => {
    const items: BreadcrumbItem[] = [
      { label: 'Home', href: '/' },
      { label: 'Middle Item' },
      { label: 'Last Item' },
    ];

    render(<Breadcrumb items={items} />);

    const middleItem = screen.getByText('Middle Item');
    expect(middleItem.tagName).toBe('SPAN');
    expect(middleItem).not.toHaveAttribute('href');
  });
});
