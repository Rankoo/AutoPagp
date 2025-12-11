import { useState } from 'react';
import { Bell, X, CheckCheck, Clock, AlertTriangle, Info, Trash2 } from 'lucide-react';
import { Button } from './ui/button';
import { Badge } from './ui/badge';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuTrigger,
} from './ui/dropdown-menu';

interface Notification {
  id: string;
  type: 'info' | 'warning' | 'success' | 'error';
  title: string;
  message: string;
  timestamp: Date;
  read: boolean;
}

const mockNotifications: Notification[] = [
  {
    id: '1',
    type: 'warning',
    title: 'Pago próximo a vencer',
    message: 'El servicio de Internet Empresarial vence en 2 días',
    timestamp: new Date(Date.now() - 1000 * 60 * 30), // hace 30 minutos
    read: false,
  },
  {
    id: '2',
    type: 'error',
    title: 'Pago vencido',
    message: 'El servicio de Hosting Web ha vencido. Por favor, actualice el pago.',
    timestamp: new Date(Date.now() - 1000 * 60 * 60 * 2), // hace 2 horas
    read: false,
  },
  {
    id: '3',
    type: 'success',
    title: 'Pago procesado',
    message: 'El pago del servicio de Telefonía Móvil se ha procesado correctamente',
    timestamp: new Date(Date.now() - 1000 * 60 * 60 * 5), // hace 5 horas
    read: false,
  },
  {
    id: '4',
    type: 'info',
    title: 'Nuevo usuario agregado',
    message: 'María González ha sido agregada al sistema',
    timestamp: new Date(Date.now() - 1000 * 60 * 60 * 24), // hace 1 día
    read: true,
  },
  {
    id: '5',
    type: 'warning',
    title: 'Actualización de seguridad',
    message: 'Se recomienda actualizar las contraseñas de acceso',
    timestamp: new Date(Date.now() - 1000 * 60 * 60 * 24 * 2), // hace 2 días
    read: true,
  },
];

export function NotificationsDropdown() {
  const [notifications, setNotifications] = useState<Notification[]>(mockNotifications);
  const [open, setOpen] = useState(false);

  const unreadCount = notifications.filter(n => !n.read).length;

  const markAsRead = (id: string) => {
    setNotifications(notifications.map(n => 
      n.id === id ? { ...n, read: true } : n
    ));
  };

  const markAllAsRead = () => {
    setNotifications(notifications.map(n => ({ ...n, read: true })));
  };

  const deleteNotification = (id: string) => {
    setNotifications(notifications.filter(n => n.id !== id));
  };

  const clearAll = () => {
    setNotifications([]);
  };

  const getIcon = (type: Notification['type']) => {
    switch (type) {
      case 'info':
        return <Info className="w-5 h-5 text-blue-600" />;
      case 'warning':
        return <AlertTriangle className="w-5 h-5 text-yellow-600" />;
      case 'success':
        return <CheckCheck className="w-5 h-5 text-green-600" />;
      case 'error':
        return <AlertTriangle className="w-5 h-5 text-red-600" />;
    }
  };

  const getBackgroundColor = (type: Notification['type']) => {
    switch (type) {
      case 'info':
        return 'bg-blue-50';
      case 'warning':
        return 'bg-yellow-50';
      case 'success':
        return 'bg-green-50';
      case 'error':
        return 'bg-red-50';
    }
  };

  const formatTimestamp = (timestamp: Date) => {
    const now = new Date();
    const diff = now.getTime() - timestamp.getTime();
    const minutes = Math.floor(diff / 1000 / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);

    if (minutes < 60) {
      return `Hace ${minutes} min`;
    } else if (hours < 24) {
      return `Hace ${hours} ${hours === 1 ? 'hora' : 'horas'}`;
    } else {
      return `Hace ${days} ${days === 1 ? 'día' : 'días'}`;
    }
  };

  return (
    <DropdownMenu open={open} onOpenChange={setOpen}>
      <DropdownMenuTrigger asChild>
        <Button variant="ghost" size="icon" className="relative">
          <Bell className="w-5 h-5" />
          {unreadCount > 0 && (
            <Badge className="absolute -top-1 -right-1 w-5 h-5 flex items-center justify-center p-0 bg-red-500 hover:bg-red-500">
              {unreadCount}
            </Badge>
          )}
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent 
        align="end" 
        className="w-96 p-0"
        sideOffset={8}
      >
        {/* Header */}
        <div className="flex items-center justify-between p-4 border-b border-gray-200">
          <div className="flex items-center gap-2">
            <h3 className="font-semibold">Notificaciones</h3>
            {unreadCount > 0 && (
              <Badge variant="secondary" className="bg-red-100 text-red-700 hover:bg-red-100">
                {unreadCount} nuevas
              </Badge>
            )}
          </div>
          {unreadCount > 0 && (
            <Button
              variant="ghost"
              size="sm"
              onClick={markAllAsRead}
              className="text-xs h-7"
            >
              Marcar todas como leídas
            </Button>
          )}
        </div>

        {/* Notifications List */}
        <div className="max-h-[400px] overflow-y-auto">
          {notifications.length === 0 ? (
            <div className="flex flex-col items-center justify-center py-12 px-4 text-center">
              <Bell className="w-12 h-12 text-gray-300 mb-3" />
              <p className="text-gray-500">No tienes notificaciones</p>
              <p className="text-xs text-gray-400 mt-1">
                Te avisaremos cuando haya algo nuevo
              </p>
            </div>
          ) : (
            <div className="divide-y divide-gray-100">
              {notifications.map((notification) => (
                <div
                  key={notification.id}
                  className={`p-4 hover:bg-gray-50 transition-colors ${
                    !notification.read ? 'bg-blue-50/30' : ''
                  }`}
                >
                  <div className="flex gap-3">
                    <div className={`flex-shrink-0 w-10 h-10 rounded-full ${getBackgroundColor(notification.type)} flex items-center justify-center`}>
                      {getIcon(notification.type)}
                    </div>
                    
                    <div className="flex-1 min-w-0">
                      <div className="flex items-start justify-between gap-2 mb-1">
                        <h4 className={`text-sm ${!notification.read ? 'font-semibold' : 'font-medium'}`}>
                          {notification.title}
                        </h4>
                        <Button
                          variant="ghost"
                          size="icon"
                          className="h-6 w-6 flex-shrink-0"
                          onClick={() => deleteNotification(notification.id)}
                        >
                          <X className="w-3.5 h-3.5" />
                        </Button>
                      </div>
                      
                      <p className="text-xs text-gray-600 mb-2 line-clamp-2">
                        {notification.message}
                      </p>
                      
                      <div className="flex items-center justify-between">
                        <div className="flex items-center gap-1 text-xs text-gray-400">
                          <Clock className="w-3 h-3" />
                          {formatTimestamp(notification.timestamp)}
                        </div>
                        
                        {!notification.read && (
                          <Button
                            variant="ghost"
                            size="sm"
                            onClick={() => markAsRead(notification.id)}
                            className="text-xs h-6 text-blue-600 hover:text-blue-700"
                          >
                            Marcar como leída
                          </Button>
                        )}
                      </div>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>

        {/* Footer */}
        {notifications.length > 0 && (
          <div className="p-3 border-t border-gray-200 bg-gray-50">
            <div className="flex gap-2">
              <Button
                variant="outline"
                size="sm"
                onClick={clearAll}
                className="flex-1 text-xs h-8"
              >
                <Trash2 className="w-3.5 h-3.5 mr-1.5" />
                Limpiar todo
              </Button>
              <Button
                variant="default"
                size="sm"
                onClick={() => setOpen(false)}
                className="flex-1 text-xs h-8 bg-gradient-to-r from-orange-600 to-orange-500"
              >
                Ver todas
              </Button>
            </div>
          </div>
        )}
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
